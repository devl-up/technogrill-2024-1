import { Button } from '@/components/ui/button.tsx';
import { usePostApiOrder } from '@/typings/api.gen.ts';
import { v4 } from 'uuid';
import { OrdersTable } from '@/routes/order/-components/orders-table.tsx';
import { useNavigate } from '@tanstack/react-router';
import { toast } from '@/components/ui/use-toast.ts';

export const component = function Index() {
  const navigate = useNavigate();
  const addOrderMutation = usePostApiOrder({
    mutation: {
      onSuccess: async (_, variables) => {
        await navigate({ to: '/order/$id', params: { id: variables.data.id } });
        toast({
          title: 'Order added'
        });
      }
    }
  });

  return (
    <div className="flex flex-col gap-8">
      <div className="text-4xl text-primary">Orders</div>
      <div className="bg-white rounded p-8 flex flex-col gap-6 shadow-lg">
        <div className="flex">
          <Button
            onClick={async () => {
              await addOrderMutation.mutateAsync({
                data: {
                  id: v4()
                }
              });
            }}>
            ADD
          </Button>
        </div>
        <OrdersTable />
      </div>
    </div>
  );
};
