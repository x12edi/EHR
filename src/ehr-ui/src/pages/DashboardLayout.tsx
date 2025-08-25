import React from 'react';
import { Layout, Menu, Button, Typography } from 'antd';
import { useAuth } from '../auth/AuthContext';
import { useNavigate, useLocation, Outlet } from 'react-router-dom';

const { Header, Sider, Content } = Layout;

export default function DashboardLayout() {
    const { user, logout } = useAuth();
    const navigate = useNavigate();
    const location = useLocation();

    return (
        <Layout style={{ minHeight: '100vh' }}>
            <Sider>
                <div style={{ color: '#fff', padding: 16, fontWeight: 600 }}>EHR</div>
                <Menu
                    theme="dark"
                    mode="inline"
                    selectedKeys={[location.pathname]}
                    onClick={({ key }) => navigate(key)}
                    items={[
                        { key: '/dashboard', label: 'Home' },
                        { key: '/patients', label: 'Patients' },
                        { key: '/appointments', label: 'Appointments' },
                        { key: '/providers', label: 'Providers' },
                    ]}
                />
            </Sider>

            <Layout>
                <Header style={{ background: '#fff', display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '0 16px' }}>
                    <Typography.Text>Welcome, {user?.username}</Typography.Text>
                    <Button onClick={logout}>Logout</Button>
                </Header>

                <Content style={{ margin: 16 }}>
                    <Outlet /> {/* Patients/Appointments/Dashboard content appears here */}
                </Content>
            </Layout>
        </Layout>
    );
}
