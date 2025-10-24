import React from 'react';
import { useParams } from 'react-router-dom';
import { useGetPropertyByIdQuery } from '../../../api/propertiesApi';

export default function PropertyDetails() {
  const { id } = useParams<{ id: string }>();
  const { data, isLoading, error } = useGetPropertyByIdQuery(id!);

  if (isLoading) return <div>Cargando...</div>;
  if (error) return <div>Error al obtener detalle</div>;
  if (!data) return <div>Propiedad no encontrada</div>;

  return (
    <div>
      <h2>{data.name}</h2>
      <p>{data.address}</p>
      <p>Precio: ${data.price.toLocaleString()}</p>
      <p>Propietario: {data.ownerName || 'N/A'}</p>
      {data.image && <img src={data.image} alt={data.name} style={{ maxWidth: '100%', borderRadius: 8 }} />}
    </div>
  );
}
