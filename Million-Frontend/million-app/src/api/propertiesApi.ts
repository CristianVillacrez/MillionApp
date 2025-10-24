import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

const BASE_URL = 'https://localhost:7209/api';

export interface RawProperty {
  idProperty: string;
  idOwner?: string;
  ownerName?: string;
  propertyName: string;
  propertyAddress: string;
  price: number;
  image?: string;
}

export interface Property {
  id: string;
  ownerId?: string;
  ownerName?: string;
  name: string;
  address: string;
  price: number;
  image?: string;
}

export interface PropertiesListResponse {
  data: Property[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface PropertyQueryParams {
  name?: string;
  address?: string;
  minPrice?: number;
  maxPrice?: number;
  page?: number;
  pageSize?: number;
}

export const propertiesApi = createApi({
  reducerPath: 'propertiesApi',
  baseQuery: fetchBaseQuery({ baseUrl: BASE_URL }),
  tagTypes: ['Property'],

  endpoints: (builder) => ({
    getProperties: builder.query<PropertiesListResponse, PropertyQueryParams | void>({
      // 👇 AQUI ESTÁ EL CAMBIO CLAVE
      query: (params?: PropertyQueryParams) => {
        const qs = new URLSearchParams();

        if (params?.name) qs.set('name', params.name);
        if (params?.address) qs.set('address', params.address);
        if (params?.minPrice !== undefined) qs.set('minPrice', String(params.minPrice));
        if (params?.maxPrice !== undefined) qs.set('maxPrice', String(params.maxPrice));
        if (params?.page) qs.set('page', String(params.page));
        if (params?.pageSize) qs.set('pageSize', String(params.pageSize));

        const q = qs.toString();
        return {
          url: `/properties${q ? `?${q}` : ''}`,
          method: 'GET',
        };
      },

      transformResponse: (response: any) => {
        const items: RawProperty[] = response?.items ?? [];
        const data: Property[] = items.map((r) => ({
          id: r.idProperty,
          ownerId: r.idOwner,
          ownerName: r.ownerName || '',
          name: r.propertyName,
          address: r.propertyAddress,
          price: r.price,
          image: r.image,
        }));

        return {
          data,
          totalCount: response?.totalCount ?? data.length,
          page: response?.page ?? 1,
          pageSize: response?.pageSize ?? data.length,
        };
      },

      providesTags: (result) =>
        result
          ? [
              ...result.data.map((p) => ({ type: 'Property' as const, id: p.id })),
              { type: 'Property' as const, id: 'LIST' },
            ]
          : [{ type: 'Property' as const, id: 'LIST' }],
    }),

    getPropertyById: builder.query<Property, string>({
      query: (id) => ({ url: `/properties/${id}`, method: 'GET' }),
      transformResponse: (r: RawProperty) => ({
        id: r.idProperty,
        ownerId: r.idOwner,
        ownerName: r.ownerName || '',
        name: r.propertyName,
        address: r.propertyAddress,
        price: r.price,
        image: r.image,
      }),
      providesTags: (result, error, id) => [{ type: 'Property', id }],
    }),
  }),
});

export const { useGetPropertiesQuery, useGetPropertyByIdQuery } = propertiesApi;
