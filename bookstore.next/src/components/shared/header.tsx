"use client"
import { Button } from '@/components/ui/button';
import Link from 'next/link';
import {notifyError, notifySuccess} from "@/app/layout";
import {signOut, isLoggedIn} from "@/app/services/user";
import React, {useEffect, useState} from "react";
import Cookies from 'js-cookie';
import { useAuth } from '@/app/auth-context';

const Header = () => {
    const { loggedIn, logout } = useAuth();

    return (
        <header className="bg-blue-900 text-white p-4 shadow-lg">
            <div className="max-w-7xl mx-auto flex justify-between items-center">
                {/* Логотип */}
                <div className="text-3xl font-semibold">
                    <Link href="/">BookStore</Link>
                </div>
                
                <nav>
                    <ul className="flex space-x-8">
                        <li>
                            <Link href="/" className="hover:text-yellow-400 transition-colors">Главная</Link>
                        </li>
                        <li>
                            <Link href="/catalog" className="hover:text-yellow-400 transition-colors">Каталог</Link>
                        </li>
                        <li>
                            <Link href="/about" className="hover:text-yellow-400 transition-colors">О нас</Link>
                        </li>
                        <li>
                            <Link href="/contact" className="hover:text-yellow-400 transition-colors">Контакты</Link>
                        </li>
                    </ul>
                </nav>
                <div className="flex items-center space-x-4">
                    <input
                        type="text"
                        placeholder="Поиск книг..."
                        className="p-2 rounded-md border-2 border-gray-400 focus:outline-none focus:border-yellow-400"
                    />
                    {loggedIn ? (
                        <Button onClick={logout} variant="outline" size="sm" className="hover:bg-yellow-600 transition-colors">
                            Выйти
                        </Button>
                    ) : (
                        <Link href={"/SignIn"} className="hover:bg-yellow-600 transition-colors">
                            Войти
                        </Link>
                    )}


                </div>
            </div>
        </header>
    );
};

export default Header;