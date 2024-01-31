import { useParams } from '@tanstack/react-router';
import {
  OrdersEnumsOrderStatus,
  useGetApiOrderId,
  usePostApiOrderApprove,
  usePostApiOrderDecline
} from '@/typings/api.gen.ts';
import { Label } from '@/components/ui/label.tsx';
import { Input } from '@/components/ui/input.tsx';
import { OrderItemsTable } from '@/routes/order/$id/-components/order-items-table.tsx';
import { AddOrderItemModal } from '@/routes/order/$id/-components/add-order-item-modal.tsx';
import { Button } from '@/components/ui/button.tsx';
import { toast } from '@/components/ui/use-toast.ts';

export const component = function Index() {
  const { id } = useParams({
    from: '/order/$id/'
  });

  const { data, refetch } = useGetApiOrderId(id, {
    query: {
      enabled: !!id
    }
  });

  const approveMutation = usePostApiOrderApprove({
    mutation: {
      onSuccess: async () => {
        await refetch();
        toast({
          title: 'Order approved'
        });
      }
    }
  });

  const declineMutation = usePostApiOrderDecline({
    mutation: {
      onSuccess: async () => {
        await refetch();
        toast({
          title: 'Order declined'
        });
      }
    }
  });

  return (
    <div className="flex flex-col gap-8">
      <div className="text-4xl text-primary">Order Detail</div>
      <div className="bg-white rounded p-8 flex flex-col gap-6 shadow-lg">
        <div className="flex flex-col gap-4 justify-start max-w-80">
          <Label htmlFor="status">Id</Label>
          <Input id="status" readOnly value={data?.id ?? ''} />
        </div>
        <div className="flex flex-col gap-4 justify-start max-w-80">
          <Label htmlFor="status">Status</Label>
          <Input id="status" readOnly value={data?.status ?? ''} />
        </div>
        <div className="flex flex-col gap-4 justify-start max-w-80">
          <Label htmlFor="status">Total</Label>
          <Input
            id="status"
            readOnly
            value={
              data?.items.reduce((total, item) => total + item.amount * item.productPrice, 0) ?? 0
            }
          />
        </div>
        <div className="flex gap-4">
          <Button
            disabled={data?.status !== OrdersEnumsOrderStatus.Pending}
            onClick={async () => {
              await approveMutation.mutateAsync({
                data: {
                  id: id
                }
              });
            }}>
            APPROVE
          </Button>
          <Button
            variant="destructive"
            disabled={data?.status !== OrdersEnumsOrderStatus.Pending}
            onClick={async () => {
              await declineMutation.mutateAsync({
                data: {
                  id: id
                }
              });
            }}>
            DECLINE
          </Button>
        </div>
      </div>
      <div className="text-4xl text-primary">Order Items</div>
      <div className="bg-white rounded p-8 flex flex-col gap-6 shadow-lg">
        <div className="flex">
          <AddOrderItemModal orderId={id} status={data?.status} existingItems={data?.items ?? []} />
        </div>
        <OrderItemsTable orderId={id} status={data?.status} items={data?.items ?? []} />
      </div>
    </div>
  );
};
