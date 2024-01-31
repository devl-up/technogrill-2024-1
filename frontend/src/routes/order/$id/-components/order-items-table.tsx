import {
  getGetApiOrderIdQueryKey,
  OrdersEnumsOrderStatus,
  OrdersQueriesGetOrderItemDto,
  usePostApiOrderDeleteItem
} from '@/typings/api.gen.ts';
import { ColumnDef } from '@tanstack/react-table';
import { DataTable } from '@/components/data-table.tsx';
import { Button } from '@/components/ui/button.tsx';
import { Trash } from 'lucide-react';
import { useQueryClient } from '@tanstack/react-query';
import { EditOrderItemModal } from '@/routes/order/$id/-components/edit-order-item-modal.tsx';
import { toast } from '@/components/ui/use-toast.ts';

export interface OrderItemsTableProps {
  readonly orderId: string;
  readonly status?: OrdersEnumsOrderStatus;
  readonly items: OrdersQueriesGetOrderItemDto[];
}
export const OrderItemsTable = ({ orderId, status, items }: OrderItemsTableProps) => {
  const queryClient = useQueryClient();

  const deleteOrderItemMutation = usePostApiOrderDeleteItem({
    mutation: {
      onSuccess: async () => {
        await queryClient.refetchQueries({
          queryKey: getGetApiOrderIdQueryKey(orderId)
        });

        toast({
          title: 'Order item deleted'
        });
      }
    }
  });

  const columns: ColumnDef<OrdersQueriesGetOrderItemDto>[] = [
    {
      accessorKey: 'productName',
      header: 'Product'
    },
    {
      accessorKey: 'productPrice',
      header: 'Price'
    },
    {
      accessorKey: 'amount',
      header: 'Amount'
    },
    {
      id: 'total',
      header: 'Total',
      cell: ({ row }) => row.original.amount * row.original.productPrice
    },
    {
      id: 'actions',
      cell: ({ row }) => {
        return (
          <div className="flex justify-center gap-2">
            <EditOrderItemModal orderId={orderId} status={status} item={row.original} />
            <Button
              variant="destructive"
              color=""
              size="icon"
              disabled={status !== OrdersEnumsOrderStatus.Pending}
              onClick={() =>
                deleteOrderItemMutation.mutateAsync({
                  data: {
                    id: orderId,
                    itemId: row.original.id
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

  return <DataTable columns={columns} data={items} />;
};
