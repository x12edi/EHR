import React, { useEffect, useState } from 'react';
import { Modal, Form, Input, DatePicker, Select, Upload, Button, message } from 'antd';
import { UploadOutlined } from '@ant-design/icons';
import type { Patient, CreatePatientDto, UpdatePatientDto } from '../services/patientService';
import dayjs from 'dayjs';
import { api } from '../lib/api';

const { Option } = Select;

type Props = {
    open: boolean;
    onClose: () => void;
    onSubmit: (values: CreatePatientDto | UpdatePatientDto) => Promise<void>;
    patient?: Patient | null; // if provided, modal is in "Edit" mode
};

export default function PatientFormModal({ open, onClose, onSubmit, patient }: Props) {
    const [form] = Form.useForm();
    const [fileList, setFileList] = useState<any[]>([]);

    useEffect(() => {
        if (patient) {
            form.setFieldsValue({
                ...patient,
                dob: patient.dob ? dayjs(patient.dob) : null,
            });
            if (patient.photoUrl) {
                setFileList([
                    {
                        uid: '-1',
                        name: 'photo',
                        status: 'done',
                        url: patient.photoUrl,
                    },
                ]);
            }
        } else {
            form.resetFields();
            setFileList([]);
        }
    }, [patient, form]);

    const handleOk = async () => {
        try {
            const values = await form.validateFields();
            if (values.dob) {
                values.dob = values.dob.toISOString();
            }
            if (patient) {
                await onSubmit({ ...values, id: patient.id });
            } else {
                await onSubmit(values);
            }
            form.resetFields();
            setFileList([]);
        } catch {
            // validation handled by AntD
        }
    };

    // Upload handler calls API
    const uploadProps = {
        name: 'file',
        action: `${api.defaults.baseURL}/files/upload`, // backend API
        headers: {
            Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
        onChange(info: any) {
            if (info.file.status === 'done') {
                const url = info.file.response.path; // backend returns { path: "/uploads/patients/xxx.jpg" }
                form.setFieldValue('photoUrl', url);
                setFileList([info.file]);
                message.success('Photo uploaded');
            } else if (info.file.status === 'error') {
                message.error('Upload failed');
            } else {
                setFileList([info.file]);
            }
        },
        beforeUpload(file: File) {
            const isImage = file.type.startsWith('image/');
            if (!isImage) {
                message.error('Only image files are allowed!');
                return Upload.LIST_IGNORE;
            }
            return true;
        },
        fileList,
    };

    return (
        <Modal
            open={open}
            title={patient ? 'Edit Patient' : 'Add Patient'}
            onCancel={() => {
                form.resetFields();
                setFileList([]);
                onClose();
            }}
            onOk={handleOk}
            destroyOnClose
            okText={patient ? 'Save Changes' : 'Create'}
        >
            <Form form={form} layout="vertical">
                <Form.Item name="mrn" label="MRN" rules={[{ required: true }]}>
                    <Input />
                </Form.Item>
                <Form.Item name="firstName" label="First Name" rules={[{ required: true }]}>
                    <Input />
                </Form.Item>
                <Form.Item name="middleName" label="Middle Name">
                    <Input />
                </Form.Item>
                <Form.Item name="lastName" label="Last Name" rules={[{ required: true }]}>
                    <Input />
                </Form.Item>
                <Form.Item name="fullNameNormalized" label="Full Name Normalized">
                    <Input />
                </Form.Item>
                <Form.Item name="dob" label="Date of Birth">
                    <DatePicker style={{ width: '100%' }} />
                </Form.Item>
                <Form.Item name="gender" label="Gender" rules={[{ required: true }]}>
                    <Select>
                        <Option value="Male">Male</Option>
                        <Option value="Female">Female</Option>
                        <Option value="Other">Other</Option>
                    </Select>
                </Form.Item>
                <Form.Item
                    name="primaryPhone"
                    label="Primary Phone"
                    rules={[
                        { required: true },
                        { pattern: /^\+?[0-9]{7,15}$/, message: 'Enter valid phone number' },
                    ]}
                >
                    <Input placeholder="+911234567890" />
                </Form.Item>
                <Form.Item name="email" label="Email" rules={[{ required: true, type: 'email' }]}>
                    <Input />
                </Form.Item>
                <Form.Item name="primaryLanguage" label="Primary Language">
                    <Input />
                </Form.Item>

                {/* Upload field */}
                <Form.Item name="photoUrl" label="Photo">
                    <Upload {...uploadProps} listType="picture">
                        <Button icon={<UploadOutlined />}>Upload Photo</Button>
                    </Upload>
                </Form.Item>

                <Form.Item name="addressesJson" label="Addresses (JSON)">
                    <Input.TextArea rows={2} />
                </Form.Item>
                <Form.Item name="identifiersJson" label="Identifiers (JSON)">
                    <Input.TextArea rows={2} />
                </Form.Item>
                <Form.Item name="demographicsJson" label="Demographics (JSON)">
                    <Input.TextArea rows={2} />
                </Form.Item>
            </Form>
        </Modal>
    );
}
