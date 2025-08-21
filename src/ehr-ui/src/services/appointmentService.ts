// src/services/appointmentService.ts
import { api } from '../lib/api';

export interface Appointment {
    id: string;
    patientId: string;
    patientName: string;
    providerId?: string;
    providerName?: string;
    departmentId?: string;
    departmentName?: string;
    locationId?: string;
    startAt: string;   // ISO date string
    endAt: string;
    status: string;
    cancelReason?: string;
    checkInAt?: string;
    checkOutAt?: string;
    isActive: boolean;
}

export interface CreateAppointmentDto {
    patientId: string;
    providerId?: string;
    departmentId?: string;
    locationId?: string;
    startAt: string;
    endAt: string;
    status: string;
    cancelReason?: string;
}

export interface UpdateAppointmentDto extends CreateAppointmentDto {
    id: string;
}

export const appointmentService = {
    async getAll(pageNumber = 1, pageSize = 10, search?: string, sortBy?: string, isAscending: boolean = true) {
        const res = await api.get('/appointments', {
            params: { pageNumber, pageSize, search, sortBy, isAscending },
        });
        return res.data;
    },

    async getById(id: string) {
        const res = await api.get(`/appointments/${id}`);
        return res.data;
    },

    async create(dto: CreateAppointmentDto) {
        const res = await api.post('/appointments', dto);
        return res.data;
    },

    async update(dto: UpdateAppointmentDto) {
        const res = await api.put(`/appointments/${dto.id}`, dto);
        return res.data;
    },

    async delete(id: string) {
        await api.delete(`/appointments/${id}`);
    },
};




