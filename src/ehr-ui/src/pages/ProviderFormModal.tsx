// src/pages/ProviderFormModal.tsx
import React, { useEffect, useState } from "react";
import { Modal, Form, Input, Select, Switch, Tabs } from "antd";
import type { Provider, CreateProviderDto, UpdateProviderDto } from "../services/providerService";
import { lookupService } from '../services/lookupService';
import TabPane from "antd/es/tabs/TabPane";
import AddressEditor from "../editors/AddressEditor";

const { Option } = Select;

type Props = {
    open: boolean;
    onClose: () => void;
    onSubmit: (values: CreateProviderDto | UpdateProviderDto) => Promise<void>;
    provider?: Provider | null;
};

export default function ProviderFormModal({ open, onClose, onSubmit, provider }: Props) {
    const [form] = Form.useForm();
    const [departments, setDepartments] = useState<{ id: string, name: string }[]>([]);

    useEffect(() => {
        lookupService.getDepartments().then(setDepartments);
        
        if (provider) {
            form.setFieldsValue({ ...provider, contactJson: provider.contactJson ? JSON.parse(provider.contactJson) : {} });
        } else {
            form.resetFields();
        }
    }, [provider, form]);

    const handleOk = async () => {
        try {
            const values = await form.validateFields();
            values.contactJson = JSON.stringify(values.contactJson || {});
            if (provider) {
                await onSubmit({ ...values, id: provider.id, userId: provider.userId });
            } else {
                await onSubmit(values);
            }
            form.resetFields();
        } catch {
            // validation handled by AntD
        }
    };

    return (
        <Modal
            open={open}
            title={provider ? "Edit Provider" : "Add Provider"}
            onCancel={() => {
                form.resetFields();
                onClose();
            }}
            onOk={handleOk}
            destroyOnClose
            okText={provider ? "Save Changes" : "Create"}
        >
            <Form form={form} layout="vertical">
                <Tabs defaultActiveKey="1">
                    <TabPane tab="General Info" key="1">
                        <Form.Item name="username" label="Username" rules={[{ required: true }]}>
                            <Input />
                        </Form.Item>
                        <Form.Item name="email" label="Email" rules={[{ required: true, type: "email" }]}>
                            <Input />
                        </Form.Item>
                        <Form.Item name="displayName" label="Display Name" rules={[{ required: true }]}>
                            <Input />
                        </Form.Item>
                        <Form.Item name="npi" label="NPI" rules={[{ required: true }]}>
                            <Input />
                        </Form.Item>
                        <Form.Item name="licenseNumber" label="License Number">
                            <Input />
                        </Form.Item>
                        <Form.Item name="specialtyCode" label="Specialty">
                            <Input />
                        </Form.Item>
                        <Form.Item name="departmentId" label="Department">
                            {/*<Select allowClear>*/}
                            {/*    */}{/* TODO: load departments from API */}
                            {/*    <Option value="dep1">Cardiology</Option>*/}
                            {/*    <Option value="dep2">Neurology</Option>*/}
                            {/*</Select>*/}
                            <Select
                                allowClear
                                options={departments.map(d => ({ label: d.name, value: d.id }))}
                            />
                        </Form.Item>
                        <Form.Item name="isActive" label="Active" valuePropName="checked" initialValue={true}>
                            <Switch />
                        </Form.Item>
                    </TabPane>
                    <TabPane tab="Contact Info" key="2">
                        <Form.Item name="contactJson" valuePropName="value">
                            <AddressEditor />
                        </Form.Item>
                    </TabPane>
                </Tabs>
            </Form>
        </Modal>
    );
}
