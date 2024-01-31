import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog.tsx';
import { Button } from '@/components/ui/button.tsx';
import { useEffect, useState } from 'react';
import { z } from 'zod';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import {
  getGetApiOrderIdQueryKey,
  OrdersEnumsOrderStatus,
  OrdersQueriesGetOrderItemDto,
  usePostApiOrderChangeItemAmount
} from '@/typings/api.gen.ts';
import { useQueryClient } from '@tanstack/react-query';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '@/components/ui/form.tsx';
import { Input } from '@/components/ui/input.tsx';
import { Pencil } from 'lucide-react';
import { toast } from '@/components/ui/use-toast.ts';

const EditOrderItemSchema = z.object({
  amount: z.coerce.number().gt(0, "Amount can't be empty")
});

export type EditOrderItemSchemaType = z.infer<typeof EditOrderItemSchema>;

export interface EditOrderItemModalProps {
  readonly orderId: string;
  readonly status?: OrdersEnumsOrderStatus;
  readonly item: OrdersQueriesGetOrderItemDto;
}

export const EditOrderItemModal = ({ orderId, status, item }: EditOrderItemModalProps) => {
  const [open, setOpen] = useState(false);
  const queryClient = useQueryClient();

  const form = useForm<EditOrderItemSchemaType>({
    resolver: zodResolver(EditOrderItemSchema),
    defaultValues: {
      amount: 0
    }
  });

  const changeOrderItemAmountMutation = usePostApiOrderChangeItemAmount({
    mutation: {
      onSuccess: async () => {
        await queryClient.refetchQueries({
          queryKey: getGetApiOrderIdQueryKey(orderId)
        });

        setOpen(false);

        toast({
          title: 'Order item changed'
        });
      }
    }
  });

  const submitHandler = async (data: EditOrderItemSchemaType) => {
    await changeOrderItemAmountMutation.mutateAsync({
      data: {
        id: orderId,
        itemId: item.id,
        amount: data.amount
      }
    });
  };

  useEffect(() => {
    if (open) {
      form.reset({
        amount: item.amount
      });
    }
  }, [form, open, item]);

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button size="icon" disabled={status !== OrdersEnumsOrderStatus.Pending}>
          <Pencil className="h-4 w-4" />
        </Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Edit Item</DialogTitle>
        </DialogHeader>
        <Form {...form}>
          <form
            id="edit-item"
            className="flex flex-col gap-4"
            onSubmit={form.handleSubmit(submitHandler)}>
            <div className="flex flex-col gap-4">
              <FormItem>
                <FormLabel>Product</FormLabel>
                <FormControl>
                  <Input
                    placeholder={'Product'}
                    type="text"
                    readOnly={true}
                    value={item.productName}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
              <FormField
                control={form.control}
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Amount</FormLabel>
                    <FormControl>
                      <Input placeholder={'Product'} type="number" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
                name={'amount'}
              />
            </div>
          </form>
        </Form>
        <DialogFooter>
          <Button
            variant={'outline'}
            onClick={() => {
              setOpen(false);
            }}>
            CANCEL
          </Button>
          <Button form="edit-item" type="submit">
            SAVE
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};
