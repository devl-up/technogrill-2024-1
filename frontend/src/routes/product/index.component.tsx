import { AddProductModal } from '@/routes/product/-components/add-product-modal.tsx';
import { ProductsTable } from '@/routes/product/-components/products-table.tsx';

export const component = function Index() {
  return (
    <div className="flex flex-col gap-8">
      <div className="text-4xl text-primary">Products</div>
      <div className="bg-white rounded p-8 flex flex-col gap-6 shadow-lg">
        <div className="flex">
          <AddProductModal />
        </div>
        <ProductsTable />
      </div>
    </div>
  );
};
