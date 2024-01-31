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
import { getGetApiProductQueryKey, usePutApiProduct } from '@/typings/api.gen.ts';
import { useQueryClient } from '@tanstack/react-query';
import { Pencil } from 'lucide-react';
import { toast } from '@/components/ui/use-toast.ts';

const EditProductSchema = z.object({
  name: z.string().min(1, "Name can't be empty").max(50, "Name can't exceed 50 characters"),
  description: z
    .string()
    .min(1, "Description can't be empty")
    .max(200, "Description can't exceed 200 characters"),
  price: z.coerce.number().gt(0, "Price can't be empty")
});

export type EditProductSchemaType = z.infer<typeof EditProductSchema>;

export interface EditProductModalProps {
  readonly id: string;
  readonly name: string;
  readonly description: string;
  readonly price: number;
}

export const EditProductModal = (props: EditProductModalProps) => {
  const [open, setOpen] = useState(false);
  const queryClient = useQueryClient();

  const form = useForm<EditProductSchemaType>({
    resolver: zodResolver(EditProductSchema),
    defaultValues: {
      name: '',
      description: '',
      price: 0
    }
  });

  const editProductsMutation = usePutApiProduct({
    mutation: {
      onSuccess: async () => {
        await queryClient.refetchQueries({
          queryKey: getGetApiProductQueryKey()
        });

        setOpen(false);
        toast({
          title: 'Product changed'
        });
      }
    }
  });

  const submitHandler = async (data: EditProductSchemaType) => {
    await editProductsMutation.mutateAsync({
      data: {
        ...data,
        id: props.id
      }
    });
  };

  useEffect(() => {
    if (open) {
      form.reset();
    }
  }, [form, open]);

  useEffect(() => {
    form.reset({
      ...props
    });
  }, [form, props]);

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <Button size="icon">
          <Pencil className="h-4 w-4" />
        </Button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Edit Product</DialogTitle>
        </DialogHeader>
        <Form {...form}>
          <form
            id="edit-product"
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
          <Button form="edit-product" type="submit">
            SAVE
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};
