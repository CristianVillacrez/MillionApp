import React from 'react';

type Filters = {
  name: string;
  address: string;
  minPrice?: number;
  maxPrice?: number;
};

export default function FilterCompound({
  value,
  onChange,
}: {
  value: Filters;
  onChange: (v: Partial<Filters>) => void;
}) {
  return (
    <form onSubmit={(e) => e.preventDefault()} style={{ display: 'flex', gap: 8, flexWrap: 'wrap' }}>
      <input placeholder="Nombre" value={value.name} onChange={(e) => onChange({ name: e.target.value })} />
      <input placeholder="Dirección" value={value.address} onChange={(e) => onChange({ address: e.target.value })} />
      <input
        placeholder="Min precio"
        type="number"
        value={value.minPrice ?? ''}
        onChange={(e) => onChange({ minPrice: e.target.value ? Number(e.target.value) : undefined })}
      />
      <input
        placeholder="Max precio"
        type="number"
        value={value.maxPrice ?? ''}
        onChange={(e) => onChange({ maxPrice: e.target.value ? Number(e.target.value) : undefined })}
      />
    </form>
  );
}
