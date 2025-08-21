import React, { useEffect, useState } from 'react';
import { Modal, Form, Input, DatePicker, Select, Upload, Button, message, Tabs } from 'antd';
import { UploadOutlined } from '@ant-design/icons';
import type { Patient, CreatePatientDto, UpdatePatientDto } from '../services/patientService';
import dayjs from 'dayjs';
import { api } from '../lib/api';

import AddressEditor from '../editors/AddressEditor';
import IdentifierEditor from '../editors/IdentifierEditor';
import DemographicsEditor from '../editors/DemographicsEditor';

const { Option } = Select;
const { TabPane } = Tabs;

type Props = {
    open: boolean;
    onClose: () => void;
    onSubmit: (values: CreatePatientDto | UpdatePatientDto) => Promise<void>;
    patient?: Patient | null;
};

export default function PatientFormModal({ open, onClose, onSubmit, patient }: Props) {
    const [form] = Form.useForm();
    const [fileList, setFileList] = useState<any[]>([]);

    useEffect(() => {
        if (patient) {
            form.setFieldsValue({
                ...patient,
                dob: patient.dob ? dayjs(patient.dob) : null,
                addressesJson: patient.addressesJson ? JSON.parse(patient.addressesJson) : {},
                identifiersJson: patient.identifiersJson ? JSON.parse(patient.identifiersJson) : [],
                demographicsJson: patient.demographicsJson ? JSON.parse(patient.demographicsJson) : {},
            });
            if (patient.photoUrl) {
                setFileList([{
                    uid: '-1',
                    name: 'photo',
                    status: 'done',
                    url: patient.photoUrl,
                }]);
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
            values.addressesJson = JSON.stringify(values.addressesJson || {});
            values.identifiersJson = JSON.stringify(values.identifiersJson || []);
            values.demographicsJson = JSON.stringify(values.demographicsJson || {});

            if (patient) {
                await onSubmit({ ...values, id: patient.id });
            } else {
                await onSubmit(values);
            }
            form.resetFields();
            setFileList([]);
        } catch {
            // validation errors handled by AntD
        }
    };

    const uploadProps = {
        name: 'file',
        action: `${api.defaults.baseURL}/files/upload`,
        headers: { Authorization: `Bearer ${localStorage.getItem('token')}` },
        onChange(info: any) {
            if (info.file.status === 'done') {
                const url = info.file.response.path;
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
            if (!file.type.startsWith('image/')) {
                message.error('Only images allowed!');
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
            onCancel={() => { form.resetFields(); setFileList([]); onClose(); }}
            onOk={handleOk}
            destroyOnClose
            okText={patient ? 'Save Changes' : 'Create'}
            width={800}
        >
            {/* ✅ One form wrapping all tabs */}
            <Form form={form} layout="vertical">
                <Tabs defaultActiveKey="1">
                    <TabPane tab="General Info" key="1">
                        <Form.Item name="mrn" label="MRN" rules={[{ required: true }]}><Input /></Form.Item>
                        <Form.Item name="firstName" label="First Name" rules={[{ required: true }]}><Input /></Form.Item>
                        <Form.Item name="middleName" label="Middle Name"><Input /></Form.Item>
                        <Form.Item name="lastName" label="Last Name" rules={[{ required: true }]}><Input /></Form.Item>
                        <Form.Item name="fullNameNormalized" label="Full Name Normalized"><Input /></Form.Item>
                        <Form.Item name="dob" label="Date of Birth"><DatePicker style={{ width: '100%' }} /></Form.Item>
                        <Form.Item name="gender" label="Gender" rules={[{ required: true }]}>
                            <Select>
                                <Option value="Male">Male</Option>
                                <Option value="Female">Female</Option>
                                <Option value="Other">Other</Option>
                            </Select>
                        </Form.Item>
                        <Form.Item name="primaryPhone" label="Primary Phone"
                            rules={[{ required: true }, { pattern: /^\+?[0-9]{7,15}$/, message: 'Invalid phone' }]}>
                            <Input placeholder="+911234567890" />
                        </Form.Item>
                        <Form.Item name="email" label="Email" rules={[{ required: true, type: 'email' }]}><Input /></Form.Item>
                        <Form.Item name="primaryLanguage" label="Primary Language"><Input /></Form.Item>
                        <Form.Item name="photoUrl" label="Photo">
                            <Upload {...uploadProps} listType="picture"><Button icon={<UploadOutlined />}>Upload</Button></Upload>
                        </Form.Item>
                    </TabPane>

                    <TabPane tab="Identifiers" key="2">
                        <Form.Item name="identifiersJson" valuePropName="value">
                            <IdentifierEditor />
                        </Form.Item>
                    </TabPane>

                    <TabPane tab="Demographics & Address" key="3">
                        <Form.Item name="addressesJson" valuePropName="value">
                            <AddressEditor />
                        </Form.Item>
                        <Form.Item name="demographicsJson" valuePropName="value">
                            <DemographicsEditor />
                        </Form.Item>
                    </TabPane>
                </Tabs>
            </Form>
        </Modal>
    );
}
