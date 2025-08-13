import React, { createContext, useContext, useEffect, useState } from 'react';

type User = { id: string; username: string; email: string };
type AuthState = {
    isAuthenticated: boolean;
    token: string | null;
    user: User | null;
    login: (token: string, user: User) => void;
    logout: () => void;
};

const AuthCtx = createContext<AuthState | null>(null);

export function AuthProvider({ children }: { children: React.ReactNode }) {
    const [token, setToken] = useState<string | null>(() => localStorage.getItem('access_token'));
    const [user, setUser] = useState<User | null>(() => {
        const raw = localStorage.getItem('user');
        return raw ? JSON.parse(raw) : null;
    });

    const login = (t: string, u: User) => {
        setToken(t);
        setUser(u);
        localStorage.setItem('access_token', t);
        localStorage.setItem('user', JSON.stringify(u));
    };

    const logout = () => {
        setToken(null);
        setUser(null);
        localStorage.removeItem('access_token');
        localStorage.removeItem('user');
        window.location.href = '/login';
    };

    // set a global axios header when token changes
    useEffect(() => {
        // lazy import to avoid circulars
        import('../lib/api').then(({ api, setAuthToken }) => {
            setAuthToken(token);
        });
    }, [token]);

    return (
        <AuthCtx.Provider value={{ isAuthenticated: !!token, token, user, login, logout }}>
            {children}
        </AuthCtx.Provider>
    );
}

export function useAuth() {
    const ctx = useContext(AuthCtx);
    if (!ctx) throw new Error('useAuth must be used within AuthProvider');
    return ctx;
}
