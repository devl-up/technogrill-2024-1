import { ColumnDef } from '@tanstack/react-table';
import {
  ProductsQueriesGetProductsDto,
  useDeleteApiProduct,
  useGetApiProduct
} from '@/typings/api.gen.ts';
import { DataTable } from '@/components/data-table.tsx';
import { Trash } from 'lucide-react';
import { Button } from '@/components/ui/button.tsx';
import { EditProductModal } from '@/routes/product/-components/edit-product-modal.tsx';
import { toast } from '@/components/ui/use-toast.ts';

export const ProductsTable = () => {
  const productsQuery = useGetApiProduct();

  const deleteProductMutation = useDeleteApiProduct({
    mutation: {
      onSuccess: async () => {
        await productsQuery.refetch();
        toast({
          title: 'Product deleted'
        });
      }
    }
  });

  const columns: ColumnDef<ProductsQueriesGetProductsDto>[] = [
    {
      accessorKey: 'id',
      header: 'Id'
    },
    {
      accessorKey: 'name',
      header: 'Name'
    },
    {
      accessorKey: 'description',
      header: 'Description'
    },
    {
      accessorKey: 'price',
      header: 'Price'
    },
    {
      id: 'actions',
      size: 0,
      cell: ({ row }) => {
        return (
          <div className="flex justify-center gap-2">
            <EditProductModal {...row.original} />
            <Button
              variant="destructive"
              color=""
              size="icon"
              onClick={() =>
                deleteProductMutation.mutateAsync({
                  data: {
                    id: row.original.id
                  }
                })
              }>
              <Trash className="h-4 w-4" />
            </Button>
          </div>
        );
      }
    }
  ];

  return <DataTable columns={columns} data={productsQuery.data ?? []} />;
};
