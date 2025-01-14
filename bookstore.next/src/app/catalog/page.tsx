"use client"
import {useEffect, useState} from "react";
import {Books} from "@/components/shared/books";
import {getAllBooks} from "@/app/services/book";

export default function Home() {
    const [books, setBooks] = useState<Book[]>([])
    const [loading, setLoading] = useState<boolean>(true)
    useEffect(() => {
        const getBooks = async () => {
            try {
                const books = await getAllBooks();
                setBooks(books);
            } catch (error) {
                console.error("Error fetching books:", error);
            } finally {
                setLoading(false);
            }
        };
        console.log(books);
        getBooks();
    }, []);
    return (
        <>
            {loading ? <h1>loading...</h1> : <Books books={books} withEditAndDelete={false}></Books>}
        </>
    );
}
