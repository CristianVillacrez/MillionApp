import React, { Suspense } from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import PropertyListContainer from './features/properties/components/PropertyList.container';

const PropertyDetails = React.lazy(() => import('./features/properties/components/PropertyDetails.lazy'));

export default function App() {
  return (
    <BrowserRouter>
      <header style={{ padding: 12, borderBottom: '1px solid #ddd' }}>
        <Link to="/">Listado de propiedades</Link>
      </header>
      <main style={{ padding: 12 }}>
        <Routes>
          <Route path="/" element={<PropertyListContainer />} />
          <Route
            path="/properties/:id"
            element={
              <Suspense fallback={<div>Cargando detalle…</div>}>
                <PropertyDetails />
              </Suspense>
            }
          />
        </Routes>
      </main>
    </BrowserRouter>
  );
}
