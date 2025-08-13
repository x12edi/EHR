import React from 'react';
import { Layout, Menu, Button, Typography } from 'antd';
import { useAuth } from '../auth/AuthContext';

const { Header, Sider, Content } = Layout;

export default function Dashboard() {
    const { user, logout } = useAuth();

    return (
        <Layout style={{ minHeight: '100vh' }}>
            <Sider>
                <div style={{ color: '#fff', padding: 16, fontWeight: 600 }}>EHR</div>
                <Menu theme="dark" mode="inline" items={[
                    { key: 'home', label: 'Home' },
                    { key: 'patients', label: 'Patients' },
                    { key: 'appointments', label: 'Appointments' },
                ]} />
            </Sider>
            <Layout>
                <Header style={{ background: '#fff', display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '0 16px' }}>
                    <Typography.Text>Welcome, {user?.username}</Typography.Text>
                    <Button onClick={logout}>Logout</Button>
                </Header>
                <Content style={{ margin: 16 }}>
                    <Typography.Title level={3}>Dashboard</Typography.Title>
                    <Typography.Paragraph>
                        This is a protected page. Next we’ll wire calls to EHR.API with the Bearer token.
                    </Typography.Paragraph>
                </Content>
            </Layout>
        </Layout>
    );
}
