import { OrdersQueriesGetOrdersDto, useDeleteApiOrder, useGetApiOrder } from '@/typings/api.gen.ts';
import { ColumnDef } from '@tanstack/react-table';
import { DataTable } from '@/components/data-table.tsx';
import { useNavigate } from '@tanstack/react-router';
import { Button } from '@/components/ui/button.tsx';
import { Eye, Trash } from 'lucide-react';
import { toast } from '@/components/ui/use-toast.ts';

export const OrdersTable = () => {
  const navigate = useNavigate();
  const ordersQuery = useGetApiOrder();

  const deleteOrderMutation = useDeleteApiOrder({
    mutation: {
      onSuccess: async () => {
        await ordersQuery.refetch();
        toast({
          title: 'Order deleted'
        });
      }
    }
  });

  const columns: ColumnDef<OrdersQueriesGetOrdersDto>[] = [
    {
      accessorKey: 'id',
      header: 'Id'
    },
    {
      accessorKey: 'status',
      header: 'Status'
    },
    {
      id: 'actions',
      cell: ({ row }) => {
        return (
          <div className="flex justify-center gap-2">
            <Button
              size="icon"
              onClick={async () => {
                await navigate({ to: '/order/$id', params: { id: row.original.id } });
              }}>
              <Eye className="h-4 w-4" />
            </Button>
            <Button
              variant="destructive"
              size="icon"
              onClick={() =>
                deleteOrderMutation.mutateAsync({
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

  return <DataTable columns={columns} data={ordersQuery.data ?? []} />;
};
