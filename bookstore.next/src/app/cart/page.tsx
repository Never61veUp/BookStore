"use client"
import {useEffect, useState} from "react";
import {CartResponse, decreaseByOne, getCart, getTotalPrice, increaseByOne} from "@/app/services/cart";
import {notifyError, notifySuccess} from "@/app/layout";

export default function Cart() {
    const [cartItems, setCartItems] = useState<CartResponse[]>([]);
    const [totalPrice, setTotalPrice] = useState(0);
    const [loading, setLoading] = useState<boolean>(true)
    useEffect(() => {
        const get = async() => {
            try {
                const cart = await getCart();
                const totalPrice = await getTotalPrice();
                setCartItems(cart)
                setTotalPrice(totalPrice)
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
    const increaseCartItem = async (bookId: string) => {
        try{
            await increaseByOne(bookId);
            const cart = await getCart();
            const totalPrice = await getTotalPrice();
            setCartItems(cart)
            setTotalPrice(totalPrice)

        }catch(error){
            if(error instanceof Error)
                notifyError(error.message);
        }
    }
    const decreaseCardItem = async (bookId: string) => {
        try{
            await decreaseByOne(bookId);
            const cart = await getCart();
            const totalPrice = await getTotalPrice();
            setCartItems(cart)
            setTotalPrice(totalPrice)

        }catch(error){
            if(error instanceof Error)
                notifyError(error.message);
        }
    }
    console.log(cartItems);
    const cartItemsArray = Object.entries(cartItems).map(([id, book]) => ({
        id,
        ...book,
    }));

    console.log(cartItemsArray);
    return (
        <>

            {loading ? <h1>loading...</h1>  : (
                <div className="min-h-screen bg-gray-100 p-6">
                    <div className="max-w-4xl mx-auto bg-white shadow-lg rounded-lg p-6">
                        <h1 className="text-2xl font-semibold mb-4 text-gray-800">
                            Корзина пользователя
                        </h1>

                        <ul className="divide-y divide-gray-200">
                            {cartItemsArray && cartItemsArray.map((cartItem: CartResponse) => (

                                <li
                                    key={cartItem.id}
                                    className="flex items-center justify-between py-4"
                                >
                                    <div>
                                        <h2 className="text-lg font-medium text-gray-700">
                                            {cartItem.Title}
                                        </h2>
                                        <p className="text-sm text-gray-500">{cartItem.AuthorFullName}</p>
                                        <p className="text-sm font-medium text-gray-900">
                                            Цена: {cartItem.Price} ₽
                                        </p>
                                    </div>

                                    <div className="text-center content-between">
                                        <div>
                                            <p className="text-sm text-gray-500">
                                                Количество: {cartItem.Count}
                                            </p>
                                            <p className="text-sm font-medium text-gray-900">
                                                Всего: {cartItem.Price * cartItem.Count} ₽
                                            </p>
                                        </div>

                                        <div>
                                            <button
                                                onClick={() => increaseCartItem(cartItem.id)}
                                                className="px-3 py-1 bg-green-500 text-white font-semibold rounded-lg shadow-md hover:bg-green-600 focus:outline-none"
                                            >
                                                +
                                            </button>
                                            <button
                                                onClick={() => decreaseCardItem(cartItem.id)}
                                                className="px-3 py-1 bg-red-500 text-white font-semibold rounded-lg shadow-md hover:bg-red-600 focus:outline-none"
                                            >
                                                -
                                            </button>
                                        </div>
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
               {totalPrice.value}₽
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

