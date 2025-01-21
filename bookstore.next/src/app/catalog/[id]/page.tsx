"use client";

import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import {useEffect, useState} from "react";
import Image from "next/image";
import {useParams} from "next/navigation";
import {getBookById} from "@/app/services/book";
import {addToCart} from "@/app/services/cart";
import {notifyError, notifySuccess} from "@/app/layout";

const BookViewPage = () => {
    const { id } = useParams();

    const [book, setBook] = useState<Book>();
    useEffect(() => {
        if(id != null){
            const getBook = async () => {
                try {
                    const book = await getBookById(id.toString());
                    setBook(book);
                } catch (error) {
                    console.error("Error fetching books:", error);
                }

            };
            getBook();
        }



    });
    const [quantity, setQuantity] = useState<number>(1);

    const handleAddToCart = async () => {
        if(id != null){
            try {
                await addToCart(id.toString());
            }catch(err) {
                if (err instanceof Error) {
                    notifyError(err.message);
                }
            }
        }

    };

    const handleBuyNow = () => {
        console.log(`Куплено ${quantity} шт. книги: ${book && book.title}`);
    };

    return (
        book && (
            <div className="max-w-5xl mx-auto p-6">
                <Card className="flex flex-col md:flex-row shadow-lg">
                    <div className="relative w-full md:w-1/2 h-96">
                        <Image
                            src={book.image.url}
                            alt={`Обложка книги ${book.title}`}
                            fill
                            className="rounded-t-md md:rounded-l-md md:rounded-tr-none object-cover"
                            sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
                            priority
                        />
                    </div>
                    <CardContent className="flex flex-col p-6 md:w-1/2">
                        <CardHeader>
                            <CardTitle className="text-2xl font-bold text-gray-800">
                                {book.title}
                            </CardTitle>
                            <p className="text-gray-600">Автор: {book.authorFullName.lastName}</p>
                        </CardHeader>
                        <Separator className="my-4"/>
                        <p className="text-gray-700 mb-4">{book.description}</p>
                        <p className="text-lg font-semibold text-gray-800 mb-4">Цена: {book.price.value} ₽</p>
                        <p className="text-sm text-gray-600 mb-6">
                            В наличии: {book.stockCount} шт.
                        </p>
                        <div className="flex items-center space-x-4 mb-6">
                            <input
                                type="number"
                                min="1"
                                max={book.stockCount}
                                value={quantity}
                                onChange={(e) => setQuantity(Number(e.target.value))}
                                className="w-20 p-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-400"
                            />
                            <Button
                                onClick={handleAddToCart}
                                variant="outline"
                                className="hover:bg-blue-100">
                                Добавить в корзину
                            </Button>
                            <Button
                                onClick={handleBuyNow}
                                className="bg-blue-600 text-white hover:bg-blue-700">
                                Купить сейчас
                            </Button>
                        </div>
                    </CardContent>
                </Card>
            </div>
        )
    );
};

export default BookViewPage;