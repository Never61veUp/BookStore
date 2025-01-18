import type { Metadata } from "next";
import { Geist, Geist_Mono } from "next/font/google";
import "./globals.css";
import {
    Menubar,
    MenubarContent,
    MenubarItem,
    MenubarMenu,
    MenubarSeparator,
    MenubarShortcut,
    MenubarTrigger,
} from "@/components/ui/menubar"
import React from "react";
import Header from "@/components/shared/header";
import Footer from "@/components/shared/footer";
import { ToastContainer, toast } from 'react-toastify';
import {Component} from "lucide-react";
import {AuthProvider} from "@/app/auth-context";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
      <AuthProvider>
          <Header />
          {children}
          <Footer />
          <ToastContainer
              position="top-right"
              autoClose={3000}
              hideProgressBar
              newestOnTop
              closeOnClick
              rtl={false}
              pauseOnFocusLoss
              draggable
              pauseOnHover
          />
      </AuthProvider>

      </body>

    </html>
  );
}
export function notifySuccess(message: string) {
    toast.success(message);
}

export function notifyError(message: string) {
    toast.error(message);
}
