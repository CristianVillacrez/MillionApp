import React, { useState, useMemo } from 'react';
import { useGetPropertiesQuery } from '../../../api/propertiesApi';
import PropertyCard from './PropertyCard.presentational';
import FilterCompound from '../../../components/Compound/FilterCompound';
import { useNavigate } from 'react-router-dom';
import styles from './PropertyCard.module.css';

type Filters = {
  name: string;
  address: string;
  minPrice?: number;
  maxPrice?: number;
  page?: number;
  pageSize?: number;
};

export default function PropertyListContainer() {
  const [filters, setFilters] = useState<Filters>({ name: '', address: '', page: 1, pageSize: 20 });
  const { data, error, isLoading, refetch } = useGetPropertiesQuery({
    name: filters.name || undefined,
    address: filters.address || undefined,
    minPrice: filters.minPrice,
    maxPrice: filters.maxPrice,
    page: filters.page,
    pageSize: filters.pageSize,
  });

  const navigate = useNavigate();
  const properties = data?.data ?? [];

  const filtered = useMemo(() => {
    return properties.filter((p) =>
      p.name.toLowerCase().includes(filters.name.toLowerCase()) &&
      p.address.toLowerCase().includes(filters.address.toLowerCase()) &&
      (filters.minPrice == null || p.price >= filters.minPrice) &&
      (filters.maxPrice == null || p.price <= filters.maxPrice)
    );
  }, [properties, filters]);

  return (
    <section className={styles.container}>
      <h1 className={styles.title}>Propiedades</h1>

      <div className={styles.filterContainer}>
        <FilterCompound
          value={filters}
          onChange={(next) => setFilters((prev) => ({ ...prev, ...next, page: 1 }))}
        />
        <button className={styles.refreshButton} onClick={() => refetch()}>
          Refrescar
        </button>
      </div>

      {isLoading && <div className={styles.message}>Cargando...</div>}
      {error && <div className={styles.message}>Error al cargar propiedades</div>}

      <div className={styles.propertyGrid}>
        {filtered.map((p) => (
          <div
            key={p.id}
            onClick={() => navigate(`/properties/${p.id}`)}
            className={styles.cardWrapper}
          >
            <PropertyCard property={p} />
          </div>
        ))}
      </div>

      <div className={styles.footer}>
        <small>
          Mostrando {filtered.length} de {data?.totalCount ?? 0} resultados
        </small>
      </div>
    </section>
  );
}
