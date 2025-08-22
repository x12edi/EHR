import { api } from '../lib/api';

export const lookupService = {
    async searchPatients(query: string) {
        const res = await api.get('/patients/search', { params: { q: query } });
        return res.data; // [{ id, name }]
    },

    async getPatientById(id: string) {
        const res = await api.get(`/patients/${id}`);
        return res.data; // [{ id, name }]
    },

    async getDepartments() {
        const res = await api.get('/departments');
        return res.data.data; // [{ id, name }]
    }
};