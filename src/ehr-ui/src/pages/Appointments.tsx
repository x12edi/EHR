import React, { useEffect, useState } from 'react';
import { Table, Button, Space, Input, Popconfirm, message, Segmented } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import type { Appointment } from '../services/appointmentService';
import { appointmentService } from '../services/appointmentService';
import AppointmentFormModal from './AppointmentFormModal';

// FullCalendar imports
import FullCalendar from '@fullcalendar/react';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';

export default function Appointments() {
    const [data, setData] = useState<Appointment[]>([]);
    const [loading, setLoading] = useState(false);
    const [search, setSearch] = useState('');
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [total, setTotal] = useState(0);
    const [sortBy, setSortBy] = useState<string | null>(null);
    const [isAscending, setIsAscending] = useState(true);

    const [modalOpen, setModalOpen] = useState(false);
    const [editing, setEditing] = useState<Appointment | null>(null);

    const [viewMode, setViewMode] = useState<'table' | 'calendar'>('table');

    const load = async () => {
        setLoading(true);
        try {
            const res = await appointmentService.getAll(pageNumber, pageSize, search, sortBy ?? undefined, isAscending);
            setData(res.data ?? res.Data ?? []);
            setTotal(res.total ?? res.Total ?? 0);
        } catch (err) {
            message.error('Failed to load appointments');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        load();
    }, [pageNumber, pageSize, search, sortBy, isAscending]);

    const handleDelete = async (id: string) => {
        try {
            await appointmentService.delete(id);
            message.success('Deleted');
            load();
        } catch {
            message.error('Delete failed');
        }
    };

    const handleSubmit = async (values: any) => {
        try {
            if (editing) {
                await appointmentService.update(values);
                message.success("Appointment updated");
            } else {
                await appointmentService.create(values);
                message.success("Appointment created");
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
                    placeholder="Search appointments..."
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
                    Add Appointment
                </Button>
                <Segmented
                    options={[
                        { label: 'Table', value: 'table' },
                        { label: 'Calendar', value: 'calendar' }
                    ]}
                    value={viewMode}
                    onChange={(val) => setViewMode(val as 'table' | 'calendar')}
                />
            </Space>

            {viewMode === 'table' ? (
                <Table<Appointment>
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
                        { title: 'Patient', dataIndex: 'patientName', sorter: true },
                        { title: 'Provider', dataIndex: 'providerName', sorter: true },
                        { title: 'Department', dataIndex: 'departmentName', sorter: true },
                        { title: 'Start', dataIndex: 'startAt', sorter: true },
                        { title: 'End', dataIndex: 'endAt', sorter: true },
                        { title: 'Status', dataIndex: 'status', sorter: true },
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
                                        title="Delete this appointment?"
                                        onConfirm={() => handleDelete(r.id)}
                                    >
                                        <Button danger size="small">Delete</Button>
                                    </Popconfirm>
                                </Space>
                            ),
                        },
                    ]}
                />
            ) : (
                <FullCalendar
                    plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
                    initialView="dayGridMonth"
                    height="80vh"
                    events={data.map(a => ({
                        id: a.id,
                        title: `${a.patientName} (${a.status})`,
                        start: a.startAt,
                        end: a.endAt,
                    }))}
                    eventClick={(info) => {
                        const appt = data.find(d => d.id === info.event.id);
                        if (appt) {
                            setEditing(appt);
                            setModalOpen(true);
                        }
                    }}
                />
            )}

            <AppointmentFormModal
                open={modalOpen}
                onClose={() => { setModalOpen(false); setEditing(null); }}
                onSubmit={handleSubmit}
                appointment={editing}
            />
        </div>
    );
}
