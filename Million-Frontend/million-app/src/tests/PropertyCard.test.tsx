import React from 'react';
import { render, screen } from '@testing-library/react';
import PropertyCard from '../features/properties/components/PropertyCard.presentational';
import '@testing-library/jest-dom';

const mock = { id: '1', name: 'Casa', address: 'Calle 1', price: 100000, image: '' };

test('muestra nombre y direccion', () => {
  render(<PropertyCard property={mock as any} />);
  expect(screen.getByText('Casa')).toBeInTheDocument();
  expect(screen.getByText('Calle 1')).toBeInTheDocument();
});
