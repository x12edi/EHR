import React, { useEffect, useState } from 'react';
import { Input } from 'antd';

export type Address = {
    line1?: string;
    line2?: string;
    city?: string;
    state?: string;
    postalCode?: string;
    country?: string;
};

interface Props {
    value?: Address;
    onChange?: (value: Address) => void;
}

export default function AddressEditor({ value, onChange }: Props) {
    const [address, setAddress] = useState<Address>(value || {});

    useEffect(() => {
        setAddress(value || {});
    }, [value]);

    const update = (key: keyof Address, val: string) => {
        const updated = { ...address, [key]: val };
        setAddress(updated);
        onChange?.(updated);
    };

    return (
        <div>
            <Input
                placeholder="Address Line 1"
                value={address.line1}
                onChange={e => update('line1', e.target.value)}
                style={{ marginBottom: 8 }}
            />
            <Input
                placeholder="Address Line 2"
                value={address.line2}
                onChange={e => update('line2', e.target.value)}
                style={{ marginBottom: 8 }}
            />
            <Input
                placeholder="City"
                value={address.city}
                onChange={e => update('city', e.target.value)}
                style={{ marginBottom: 8 }}
            />
            <Input
                placeholder="State"
                value={address.state}
                onChange={e => update('state', e.target.value)}
                style={{ marginBottom: 8 }}
            />
            <Input
                placeholder="Postal Code"
                value={address.postalCode}
                onChange={e => update('postalCode', e.target.value)}
                style={{ marginBottom: 8 }}
            />
            <Input
                placeholder="Country"
                value={address.country}
                onChange={e => update('country', e.target.value)}
            />
        </div>
    );
}
