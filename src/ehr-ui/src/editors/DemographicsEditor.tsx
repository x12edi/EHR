import React, { useEffect, useState } from 'react';
import { Select, Input } from 'antd';

const { Option } = Select;

type Demographics = {
    maritalStatus?: string;
    ethnicity?: string;
    race?: string;
    religion?: string;
};

interface Props {
    value?: Demographics;
    onChange?: (value: Demographics) => void;
}

export default function DemographicsEditor({ value, onChange }: Props) {
    const [demographics, setDemographics] = useState<Demographics>(value || {});

    useEffect(() => {
        setDemographics(value || {});
    }, [value]);

    const update = (key: keyof Demographics, val: string) => {
        const updated = { ...demographics, [key]: val };
        setDemographics(updated);
        onChange?.(updated);
    };

    return (
        <div>
            <Select
                style={{ width: '100%', marginBottom: 8 }}
                placeholder="Marital Status"
                value={demographics.maritalStatus}
                onChange={val => update('maritalStatus', val)}
            >
                <Option value="Single">Single</Option>
                <Option value="Married">Married</Option>
                <Option value="Divorced">Divorced</Option>
                <Option value="Widowed">Widowed</Option>
            </Select>

            <Input
                style={{ marginBottom: 8 }}
                placeholder="Ethnicity"
                value={demographics.ethnicity}
                onChange={e => update('ethnicity', e.target.value)}
            />

            <Input
                style={{ marginBottom: 8 }}
                placeholder="Race"
                value={demographics.race}
                onChange={e => update('race', e.target.value)}
            />

            <Input
                placeholder="Religion"
                value={demographics.religion}
                onChange={e => update('religion', e.target.value)}
            />
        </div>
    );
}
