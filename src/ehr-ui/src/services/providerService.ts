// src/services/providerService.ts
import { api } from "../lib/api";

export interface Provider {
    id: string;
    userId: string;
    username: string;
    email: string;
    displayName: string;
    npi: string;
    licenseNumber: string;
    specialtyCode: string;
    departmentId?: string;
    departmentName?: string;
    contactJson?: string;
    isActive: boolean;
}

export interface CreateProviderDto {
    username: string;
    email: string;
    displayName: string;
    npi: string;
    licenseNumber: string;
    specialtyCode: string;
    departmentId?: string;
    contactJson?: string;
}

export interface UpdateProviderDto extends CreateProviderDto {
    id: string;
    userId: string;
}

export const providerService = {
    async getAll(
        pageNumber: number,
        pageSize: number,
        search?: string,
        sortBy?: string,
        isAscending?: boolean
    ) {
        const res = await api.get("/providers", {
            params: { pageNumber, pageSize, search, sortBy, isAscending },
        });
        return res.data;
    },
    async create(data: CreateProviderDto) {
        const res = await api.post("/providers", data);
        return res.data;
    },
    async update(data: UpdateProviderDto) {
        const res = await api.put(`/providers/${data.id}`, data);
        return res.data;
    },
    async delete(id: string) {
        const res = await api.delete(`/providers/${id}`);
        return res.data;
    },
};
