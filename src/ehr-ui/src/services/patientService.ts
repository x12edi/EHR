import { api } from '../lib/api';

export interface Patient {
    id: string;
    mrn: string;
    firstName: string;
    lastName: string;
    dob?: string;
    gender: string;
    primaryPhone?: string;
    email?: string;
}

export interface CreatePatientDto {
    mrn: string;
    firstName: string;
    lastName: string;
    dob?: string;
    gender: string;
    primaryPhone?: string;
    email?: string;
}

export interface UpdatePatientDto extends CreatePatientDto {
    id: string;
}

export const patientService = {
    async getAll(pageNumber = 1, pageSize = 10, search?: string, sortBy?: string, isAscending: boolean = true) {
        const res = await api.get('/patients', {
            params: { pageNumber, pageSize, search, sortBy, isAscending },
        });
        return res.data;
    },

    async getById(id: string) {
        const res = await api.get(`/patients/${id}`);
        return res.data;
    },

    async create(dto: CreatePatientDto) {
        const res = await api.post('/patients', dto);
        return res.data;
    },

    async update(dto: UpdatePatientDto) {
        const res = await api.put(`/patients/${dto.id}`, dto);
        return res.data;
    },

    async delete(id: string) {
        await api.delete(`/patients/${id}`);
    },
};
