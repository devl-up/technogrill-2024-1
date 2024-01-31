import { Link, Outlet, RootRoute } from '@tanstack/react-router';
import { Store, Wallet } from 'lucide-react';
import { Toaster } from '@/components/ui/toaster.tsx';

const links = [
  {
    to: '/product',
    label: 'Products',
    icon: <Store className="h-8 w-8 text-primary" />
  },
  {
    to: '/order',
    label: 'Orders',
    icon: <Wallet className="h-8 w-8 text-primary" />
  }
];

export const Route = new RootRoute({
  component: () => (
    <>
      <div className="h-screen w-screen flex flex-col divide-y">
        <div className="flex px-8 py-4 gap-8 items-center justify-between shadow-md z-10">
          <div className="text-2xl text-primary">TechnoGrill</div>
        </div>
        <div className="flex flex-grow divide-x">
          <div className="w-80 flex flex-col pt-8 px-6 gap-6">
            {links.map(({ to, label, icon }) => (
              <Link
                key={to}
                className="flex transition-all ease-in items-center gap-5 text-lg text-center px-4 py-2 rounded-xl text-primary hover:bg-accent [&.active]:bg-accent"
                to={to}>
                {icon} {label}
              </Link>
            ))}
          </div>
          <div className="flex-grow p-8 bg-accent">
            <Outlet />
          </div>
        </div>
      </div>
      <Toaster />
    </>
  )
});
