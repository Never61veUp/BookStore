"use client"
import {useEffect, useState} from "react";
import {CartResponse, getCart} from "@/app/services/cart";
import {notifyError, notifySuccess} from "@/app/layout";

export default function Cart() {
    const [cartItems, setCartItems] = useState<CartResponse[]>([]);
    const [loading, setLoading] = useState<boolean>(true)
    useEffect(() => {
        const get = async() => {
            try {
                const cart = await getCart();
                setCartItems(cart)
                notifySuccess("Успешно")
            }catch (error){
                if(error instanceof Error)
                    notifyError(error.message);
            }finally {
                setLoading(false);
            }
        }
        get();
    }, []);
    console.log(cartItems);
    return (
        <>

            {loading ? <h1>loading...</h1>  : (
                <div className="min-h-screen bg-gray-100 p-6">
                    <div className="max-w-4xl mx-auto bg-white shadow-lg rounded-lg p-6">
                        <h1 className="text-2xl font-semibold mb-4 text-gray-800">
                            Корзина пользователя
                        </h1>

                        <ul className="divide-y divide-gray-200">
                            {cartItems && cartItems.map((cartItem: CartResponse) => (

                                <li
                                    key={cartItem.bookId}
                                    className="flex items-center justify-between py-4"
                                >
                                    <div>
                                        <h2 className="text-lg font-medium text-gray-700">
                                            {cartItem.books.title}
                                        </h2>
                                        <p className="text-sm text-gray-500">{cartItem.books.authorFullName}</p>
                                        <p className="text-sm font-medium text-gray-900">
                                            Цена: {} ₽
                                        </p>
                                    </div>

                                    <div className="text-center">
                                        <p className="text-sm text-gray-500">
                                            Количество: {cartItem.books.count}
                                        </p>
                                        <p className="text-sm font-medium text-gray-900">
                                            Всего: {} ₽
                                        </p>
                                    </div>
                                </li>
                            ))}
                        </ul>

                        <div className="mt-6 border-t border-gray-200 pt-4">
                            <div className="flex justify-between items-center">
            <span className="text-lg font-semibold text-gray-700">
              Общая стоимость:
            </span>
                                <span className="text-lg font-bold text-green-600">
               ₽
            </span>
                            </div>
                            <button
                                className="w-full mt-4 py-2 px-4 bg-blue-600 text-white rounded-lg shadow hover:bg-blue-700 transition">
                                Оформить заказ
                            </button>
                        </div>
                    </div>
                </div>
            )}

        </>
    );
}

