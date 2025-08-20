import React, { useEffect, useState } from 'react';
import { Table, Button, Space, message, Input, Popconfirm } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import type { Patient } from '../services/patientService';
import { patientService } from '../services/patientService';
import PatientFormModal from './PatientFormModal';

export default function Patients() {
    const [data, setData] = useState<Patient[]>([]);
    const [loading, setLoading] = useState(false);
    const [search, setSearch] = useState('');
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [total, setTotal] = useState(0);
    const [sortBy, setSortBy] = useState<string | null>(null);
    const [isAscending, setIsAscending] = useState(true);

    const [modalOpen, setModalOpen] = useState(false);
    const [editing, setEditing] = useState<Patient | null>(null);

    const load = async () => {
        setLoading(true);
        try {
            const res = await patientService.getAll(pageNumber, pageSize, search, sortBy ?? undefined, isAscending);
            setData(res.data ?? res.Data ?? []); // adjust based on API response shape
            setTotal(res.total ?? res.Total ?? 0);
        } catch (err: any) {
            message.error('Failed to load patients');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        load();
    }, [pageNumber, pageSize, search, sortBy, isAscending]);

    const handleDelete = async (id: string) => {
        try {
            await patientService.delete(id);
            message.success('Deleted');
            load();
        } catch {
            message.error('Delete failed');
        }
    };

    const handleSubmit = async (values: any) => {
        try {
            if (editing) {
                await patientService.update(values);
                message.success("Patient updated");
            } else {
                await patientService.create(values);
                message.success("Patient created");
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
                    placeholder="Search patients..."
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
                    Add Patient
                </Button>
            </Space>

            <Table<Patient>
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
                    }
                }}
                onChange={(pagination, filters, sorter: any) => {
                    if (sorter?.field) {
                        setSortBy(sorter.field);
                        setIsAscending(sorter.order === 'ascend');
                    } else {
                        setSortBy(null);
                    }
                }}
                columns={[
                    { title: 'MRN', dataIndex: 'MRN', sorter: true },
                    { title: 'First Name', dataIndex: 'FirstName', sorter: true },
                    { title: 'Middle Name', dataIndex: 'MiddleName' },
                    { title: 'Last Name', dataIndex: 'LastName', sorter: true },
                    { title: 'DOB', dataIndex: 'DOB', sorter: true },
                    { title: 'Gender', dataIndex: 'Gender', sorter: true },
                    { title: 'Phone', dataIndex: 'PrimaryPhone' },
                    { title: 'Email', dataIndex: 'Email' },
                    { title: 'Language', dataIndex: 'PrimaryLanguage' },
                    {
                        title: 'Actions',
                        render: (_, r) => (
                            <Space>
                                <Button size="small" onClick={() => {
                                    setEditing(r);
                                    setModalOpen(true);
                                }}>
                                    Edit
                                </Button>
                                <Popconfirm
                                    title="Delete this patient?"
                                    onConfirm={() => handleDelete(r.id)}
                                >
                                    <Button danger size="small">Delete</Button>
                                </Popconfirm>
                            </Space>
                        ),
                    },
                ]}
            />

            <PatientFormModal
                open={modalOpen}
                onClose={() => { setModalOpen(false); setEditing(null); }}
                onSubmit={handleSubmit}
                patient={editing}
            />
        </div>
    );
}
