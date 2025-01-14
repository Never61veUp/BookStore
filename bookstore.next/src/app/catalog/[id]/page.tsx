"use client";

import { useParams } from 'next/navigation';
import { getBookById} from "@/app/services/book";
import {useEffect, useState} from "react";

const BookPage = () => {
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

    return (
      <>
          {book && <p>{book && book.title}</p>}
      </>
    );
};

export default BookPage;