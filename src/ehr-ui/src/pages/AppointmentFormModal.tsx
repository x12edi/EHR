import React, { useEffect, useState } from 'react';
import { Modal, Form, DatePicker, Select, Input } from 'antd';
import debounce from 'lodash.debounce';
import dayjs from 'dayjs';
import { lookupService } from '../services/lookupService';
import type { Appointment, CreateAppointmentDto, UpdateAppointmentDto } from '../services/appointmentService';

const { Option } = Select;

export default function AppointmentFormModal({ open, onClose, onSubmit, appointment }: any) {
    const [form] = Form.useForm();
    const [patients, setPatients] = useState<{ id: string, name: string }[]>([]);
    const [departments, setDepartments] = useState<{ id: string, name: string }[]>([]);

    useEffect(() => {
        lookupService.getDepartments().then(setDepartments);

        if (appointment) {
            form.setFieldsValue({
                ...appointment,
                startAt: dayjs(appointment.startAt),
                endAt: dayjs(appointment.endAt),
            });

            // Ensure the selected patient is loaded into the dropdown
            (async () => {
                const patientRes = await lookupService.getPatientById(appointment.patientId);
                setPatients([patientRes]); // Populate with just the current patient
            })();

        } else {
            form.resetFields();
        }
    }, [appointment, form]);

    const searchPatients = debounce(async (val: string) => {
        if (val) {
            const res = await lookupService.searchPatients(val);
            setPatients(res);
        }
    }, 400);

    const handleOk = async () => {
        try {
            const values = await form.validateFields();
            values.startAt = values.startAt.toISOString();
            values.endAt = values.endAt.toISOString();

            console.log('Submitting appointment:', values);

            if (appointment) {
                await onSubmit({ ...values, id: appointment.id });
            } else {
                await onSubmit(values);
            }
            form.resetFields();
        } catch { }
    };

    return (
        <Modal
            open={open}
            title={appointment ? "Edit Appointment" : "Add Appointment"}
            onCancel={() => { form.resetFields(); onClose(); }}
            onOk={handleOk}
            destroyOnClose
            okText={appointment ? "Save Changes" : "Create"}
        >
            <Form form={form} layout="vertical">
                <Form.Item name="patientId" label="Patient" rules={[{ required: true }]}>
                    <Select
                        showSearch
                        filterOption={false}
                        onSearch={searchPatients}
                        placeholder="Search patient..."
                        options={patients.map(p => ({ label: `${p.firstName} ${p.lastName}`, value: p.id }))}
                    />
                </Form.Item>
                <Form.Item name="departmentId" label="Department">
                    <Select
                        allowClear
                        options={departments.map(d => ({ label: d.name, value: d.id }))}
                    />
                </Form.Item>
                <Form.Item name="providerId" label="Created By" hidden>
                    
                </Form.Item>
                <Form.Item name="providerName" label="Created By">
                    <Input disabled></Input>
                </Form.Item>
                <Form.Item name="startAt" label="Start At" rules={[{ required: true }]}>
                    <DatePicker showTime style={{ width: '100%' }} />
                </Form.Item>
                <Form.Item name="endAt" label="End At" rules={[{ required: true }]}>
                    <DatePicker showTime style={{ width: '100%' }} />
                </Form.Item>
                <Form.Item name="status" label="Status" rules={[{ required: true }]}>
                    <Select>
                        <Option value="Scheduled">Scheduled</Option>
                        <Option value="CheckedIn">Checked In</Option>
                        <Option value="Completed">Completed</Option>
                        <Option value="Cancelled">Cancelled</Option>
                    </Select>
                </Form.Item>
                <Form.Item name="cancelReason" label="Cancel Reason">
                    <Input />
                </Form.Item>
            </Form>
        </Modal>
    );
}
