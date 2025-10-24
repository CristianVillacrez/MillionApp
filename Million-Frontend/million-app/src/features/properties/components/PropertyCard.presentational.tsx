import React from 'react';
import { Property } from '../../../api/propertiesApi';
import styles from './PropertyCard.module.css';

export default function PropertyCard({ property }: { property: Property }) {
  return (
    <article className={styles.card}>
      <div className={styles.imageWrapper}>
        {property.image ? (
          <img
            src={property.image}
            alt={property.name}
            className={styles.image}
          />
        ) : (
          <div className={styles.noImage}>Sin imagen</div>
        )}
      </div>

      <div className={styles.info}>
        <h3 className={styles.name}>{property.name}</h3>
        <p className={styles.address}>{property.address}</p>
        <p className={styles.price}>${property.price.toLocaleString()}</p>
      </div>
    </article>
  );
}
