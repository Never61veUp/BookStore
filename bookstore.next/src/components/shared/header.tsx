"use client"
import Link from 'next/link';
import React from "react";
import {ProfileDropDownMenu} from "@/components/shared/profileDropDownMenu";

const Header = () => {

    return (
        <header className="bg-blue-900 p-4 shadow-lg">
            <div className="max-w-7xl mx-auto flex justify-between items-center">
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
                    <ProfileDropDownMenu/>
                </div>
            </div>
        </header>
    );
};

export default Header;