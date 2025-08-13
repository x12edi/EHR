import axios from 'axios';

export const identity = axios.create({
    baseURL: import.meta.env.VITE_IDENTITY_URL,
});

export const api = axios.create({
    baseURL: import.meta.env.VITE_API_URL,
});

export function setAuthToken(token: string | null) {
    if (token) {
        api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    } else {
        delete api.defaults.headers.common['Authorization'];
    }
}
