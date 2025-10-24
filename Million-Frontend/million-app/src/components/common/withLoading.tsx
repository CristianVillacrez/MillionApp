import React, { ComponentType, FC } from 'react';

export default function withLoading<P extends Record<string, any>>(
  Component: ComponentType<P>
) {
  const WrappedComponent: FC<P & { isLoading?: boolean }> = (props) => {
    const { isLoading, ...rest } = props;

    if (isLoading) return <div>Cargando...</div>;
    return <Component {...(rest as P)} />;
  };

  WrappedComponent.displayName = `withLoading(${Component.displayName || Component.name || 'Component'})`;
  return WrappedComponent;
}
