import { z } from 'zod';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { Button } from '@/components/ui/button.tsx';
import { Input } from '@/components/ui/input.tsx';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '@/components/ui/form.tsx';
import { Textarea } from '@/components/ui/textarea.tsx';
import {
  Dialog,
  DialogContent,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog.tsx';
import { useEffect, useState } from 'react';
import { getGetApiProductQueryKey, usePostApiProduct } from '@/typings/api.gen.ts';
import { useQueryClient } from '@tanstack/react-query';
import { v4 } from 'uuid';
import { toast } from '@/components/ui/use-toast.ts';

const AddProductSchema = z.object({
  name: z.string().min(1, "Name can't be empty").max(50, "Name can't exceed 50 characters"),
  description: z
    .string()
    .min(1, "Description can't be empty")
    .max(200, "Description can't exceed 200 characters"),
  price: z.coerce.number().gt(0, "Price can't be empty")
});

export type AddProductSchemaType = z.infer<typeof AddProductSchema>;

export const AddProductModal = () => {
  const [open, setOpen] = useState(false);
  const queryClient = useQueryClient();

  const form = useForm<AddProductSchemaType>({
    resolver: zodResolver(AddProductSchema),
    defaultValues: {
      name: '',
      description: '',
      price: 0
    }
  });

  const addProductsMutation = usePostApiProduct({
    mutation: {
      onSuccess: async () => {
        await queryClient.refetchQueries({
          queryKey: getGetApiProductQueryKey()
        });

        setOpen(false);
        toast({
          title: 'Product added'
        });
      }
    }
  });

  const submitHandler = async (data: AddProductSchemaType) => {
    await addProductsMutation.mutateAsync({
      data: {
        ...data,
        id: v4()
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
        <Button>ADD</Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Add Product</DialogTitle>
        </DialogHeader>
        <Form {...form}>
          <form
            id="add-product"
            className="flex flex-col gap-4"
            onSubmit={form.handleSubmit(submitHandler)}>
            <div className="flex flex-col gap-4">
              <FormField
                control={form.control}
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Name</FormLabel>
                    <FormControl>
                      <Input placeholder={'Name'} type="text" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
                name={'name'}
              />
              <FormField
                control={form.control}
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Description</FormLabel>
                    <FormControl>
                      <Textarea placeholder={'Description'} rows={3} {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
                name={'description'}
              />
              <FormField
                control={form.control}
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Price</FormLabel>
                    <FormControl>
                      <Input placeholder={'Price'} type="number" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
                name={'price'}
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
          <Button form="add-product" type="submit">
            SAVE
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};
