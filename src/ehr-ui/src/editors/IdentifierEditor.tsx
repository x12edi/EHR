import React, { useEffect, useState } from 'react';
import { Input, Button, Space } from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';

type Identifier = {
    system: string;   // e.g. "SSN", "Passport", "MRN"
    value: string;
};

interface Props {
    value?: Identifier[];
    onChange?: (value: Identifier[]) => void;
}

export default function IdentifierEditor({ value = [], onChange }: Props) {
    const [identifiers, setIdentifiers] = useState<Identifier[]>(value);

    useEffect(() => {
        setIdentifiers(value || []);
    }, [value]);

    const addIdentifier = () => {
        const updated = [...identifiers, { system: '', value: '' }];
        setIdentifiers(updated);
        onChange?.(updated);
    };

    const updateIdentifier = (index: number, key: keyof Identifier, val: string) => {
        const updated = identifiers.map((id, i) =>
            i === index ? { ...id, [key]: val } : id
        );
        setIdentifiers(updated);
        onChange?.(updated);
    };

    const removeIdentifier = (index: number) => {
        const updated = identifiers.filter((_, i) => i !== index);
        setIdentifiers(updated);
        onChange?.(updated);
    };

    return (
        <div>
            {identifiers.map((id, i) => (
                <Space key={i} style={{ display: 'flex', marginBottom: 8 }}>
                    <Input
                        placeholder="System (e.g. SSN, Passport)"
                        value={id.system}
                        onChange={e => updateIdentifier(i, 'system', e.target.value)}
                    />
                    <Input
                        placeholder="Value"
                        value={id.value}
                        onChange={e => updateIdentifier(i, 'value', e.target.value)}
                    />
                    <Button danger icon={<DeleteOutlined />} onClick={() => removeIdentifier(i)} />
                </Space>
            ))}
            <Button onClick={addIdentifier} type="dashed" block icon={<PlusOutlined />}>
                Add Identifier
            </Button>
        </div>
    );
}
