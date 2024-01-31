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
  useGetApiProduct,
  usePostApiOrderAddItem
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
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue
} from '@/components/ui/select.tsx';
import { toast } from '@/components/ui/use-toast.ts';

const AddOrderItemSchema = z.object({
  productId: z.string().uuid('Invalid product').min(1, "Product can't be empty"),
  amount: z.coerce.number().gt(0, "Amount can't be empty")
});

export type AddOrderItemSchemaType = z.infer<typeof AddOrderItemSchema>;

export interface AddOrderItemModalProps {
  readonly orderId: string;
  readonly status?: OrdersEnumsOrderStatus;
  readonly existingItems: OrdersQueriesGetOrderItemDto[];
}

export const AddOrderItemModal = ({ orderId, status, existingItems }: AddOrderItemModalProps) => {
  const [open, setOpen] = useState(false);
  const queryClient = useQueryClient();

  const form = useForm<AddOrderItemSchemaType>({
    resolver: zodResolver(AddOrderItemSchema),
    defaultValues: {
      productId: '',
      amount: 0
    }
  });

  const addOrderItemMutation = usePostApiOrderAddItem({
    mutation: {
      onSuccess: async () => {
        await queryClient.refetchQueries({
          queryKey: getGetApiOrderIdQueryKey(orderId)
        });

        setOpen(false);
        toast({
          title: 'Order item added'
        });
      }
    }
  });

  const getProductsQuery = useGetApiProduct();

  const submitHandler = async (data: AddOrderItemSchemaType) => {
    await addOrderItemMutation.mutateAsync({
      data: {
        ...data,
        id: orderId
      }
    });
  };

  useEffect(() => {
    if (open) {
      form.reset();
    }
  }, [form, open]);

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button disabled={status !== OrdersEnumsOrderStatus.Pending}>ADD</Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Add Item</DialogTitle>
        </DialogHeader>
        <Form {...form}>
          <form
            id="add-item"
            className="flex flex-col gap-4"
            onSubmit={form.handleSubmit(submitHandler)}>
            <div className="flex flex-col gap-4">
              <FormField
                control={form.control}
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Product</FormLabel>
                    <Select onValueChange={field.onChange}>
                      <FormControl>
                        <SelectTrigger>
                          <SelectValue placeholder="Select a product" />
                        </SelectTrigger>
                      </FormControl>
                      <SelectContent>
                        {getProductsQuery.data
                          ?.filter((p) => !existingItems.some((i) => i.productId === p.id))
                          .map((p) => (
                            <SelectItem key={p.id} value={p.id}>
                              {p.name}
                            </SelectItem>
                          ))}
                      </SelectContent>
                    </Select>
                    <FormMessage />
                  </FormItem>
                )}
                name={'productId'}
              />
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
          <Button form="add-item" type="submit">
            SAVE
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};
