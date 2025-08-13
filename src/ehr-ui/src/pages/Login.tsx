import React, { useState } from 'react';
import { Card, Form, Input, Button, message, Typography } from 'antd';
import { identity } from '../lib/api';
import { useAuth } from '../auth/AuthContext';

export default function Login() {
    const [loading, setLoading] = useState(false);
    const { login } = useAuth();

    const onFinish = async (values: any) => {
        setLoading(true);
        try {
            const res = await identity.post('/api/account/login', values);
            const { accessToken, user } = res.data;
            login(accessToken, user);
            message.success('Logged in');
            window.location.href = '/dashboard';
        } catch (err: any) {
            message.error(err?.response?.data?.message ?? 'Login failed');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={{ minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <Card title="EHR Login" style={{ width: 360 }}>
                <Form layout="vertical" onFinish={onFinish}>
                    <Form.Item label="Username" name="username" rules={[{ required: true }]}>
                        <Input autoFocus />
                    </Form.Item>
                    <Form.Item label="Password" name="password" rules={[{ required: true }]}>
                        <Input.Password />
                    </Form.Item>
                    <Button type="primary" htmlType="submit" block loading={loading}>
                        Sign in
                    </Button>
                    <Typography.Paragraph type="secondary" style={{ marginTop: 12 }}>
                        No account? Use the register API or seed a user.
                    </Typography.Paragraph>
                </Form>
            </Card>
        </div>
    );
}
