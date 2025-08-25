// src/pages/Providers.tsx
import React, { useEffect, useState } from "react";
import { Table, Button, Space, Input, Popconfirm, message } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import type { Provider } from "../services/providerService";
import { providerService } from "../services/providerService";
import ProviderFormModal from "./ProviderFormModal";

export default function Providers() {
    const [data, setData] = useState<Provider[]>([]);
    const [loading, setLoading] = useState(false);
    const [search, setSearch] = useState("");
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [total, setTotal] = useState(0);
    const [sortBy, setSortBy] = useState<string | null>(null);
    const [isAscending, setIsAscending] = useState(true);

    const [modalOpen, setModalOpen] = useState(false);
    const [editing, setEditing] = useState<Provider | null>(null);

    const load = async () => {
        setLoading(true);
        try {
            const res = await providerService.getAll(
                pageNumber,
                pageSize,
                search,
                sortBy ?? undefined,
                isAscending
            );
            setData(res.data ?? res.Data ?? []);
            setTotal(res.total ?? res.Total ?? 0);
        } catch {
            message.error("Failed to load providers");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        load();
    }, [pageNumber, pageSize, search, sortBy, isAscending]);

    const handleDelete = async (id: string) => {
        try {
            await providerService.delete(id);
            message.success("Deleted");
            load();
        } catch {
            message.error("Delete failed");
        }
    };

    const handleSubmit = async (values: any) => {
        try {
            if (editing) {
                await providerService.update({ ...values, id: editing.id, userId: editing.userId });
                message.success("Provider updated");
            } else {
                await providerService.create(values);
                message.success("Provider created");
            }
            setModalOpen(false);
            setEditing(null);
            load();
        } catch {
            message.error("Save failed");
        }
    };

    return (
        <div>
            <Space style={{ marginBottom: 16 }}>
                <Input.Search
                    placeholder="Search providers..."
                    onSearch={setSearch}
                    allowClear
                />
                <Button
                    type="primary"
                    icon={<PlusOutlined />}
                    onClick={() => {
                        setEditing(null);
                        setModalOpen(true);
                    }}
                >
                    Add Provider
                </Button>
            </Space>

            <Table<Provider>
                rowKey="id"
                loading={loading}
                dataSource={data}
                pagination={{
                    current: pageNumber,
                    pageSize,
                    total,
                    showSizeChanger: true,
                    onChange: (page, size) => {
                        setPageNumber(page);
                        setPageSize(size);
                    },
                }}
                onChange={(pagination, filters, sorter: any) => {
                    if (sorter?.field) {
                        setSortBy(sorter.field);
                        setIsAscending(sorter.order === "ascend");
                    } else {
                        setSortBy(null);
                    }
                }}
                columns={[
                    { title: "Username", dataIndex: "username", sorter: true },
                    { title: "Email", dataIndex: "email", sorter: true },
                    { title: "Display Name", dataIndex: "displayName", sorter: true },
                    { title: "NPI", dataIndex: "npi", sorter: true },
                    { title: "Specialty", dataIndex: "specialtyCode", sorter: true },
                    { title: "Department", dataIndex: "departmentName", sorter: true },
                    {
                        title: "Actions",
                        render: (_, r) => (
                            <Space>
                                <Button
                                    size="small"
                                    onClick={() => {
                                        setEditing(r);
                                        setModalOpen(true);
                                    }}
                                >
                                    Edit
                                </Button>
                                <Popconfirm
                                    title="Delete this provider?"
                                    onConfirm={() => handleDelete(r.id)}
                                >
                                    <Button danger size="small">
                                        Delete
                                    </Button>
                                </Popconfirm>
                            </Space>
                        ),
                    },
                ]}
            />

            <ProviderFormModal
                open={modalOpen}
                onClose={() => {
                    setModalOpen(false);
                    setEditing(null);
                }}
                onSubmit={handleSubmit}
                provider={editing}
            />
        </div>
    );
}
