"use client"
import {useEffect, useState} from "react";
import {Books} from "@/components/shared/books";
import {BookRequest, createBook, deleteBook, getAllBooks, updateBook} from "@/app/services/book";
import {Button} from "@/components/ui/button";
import {CreateUpdateBook, Mode} from "@/components/shared/CreateUpdateBook";

export default function Home() {
    const defaultValues = {
        title: "",
        description: "",
        price: {value:1},
        stockCount: 0
    } as Book;
    const [values, setValues] = useState<Book>(defaultValues);
    const [books, setBooks] = useState<Book[]>([])
    const [loading, setLoading] = useState<boolean>(true)
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const [mode, setMode] = useState<Mode>(Mode.Create);

    const handleCreateBook = async (request: BookRequest, file: File | null) => {
        if(file){
            await createBook(request, file);
            closeModal();

            const books = await getAllBooks();
            setBooks(books);
        }

    };

    const handleUpdateBook = async (id: string, request: BookRequest, file: File | null) => {
        await updateBook(id, request, file);
        closeModal();

        const books = await getAllBooks();
        setBooks(books);
    }

    const handleDeleteBook = async (id: string) => {

        await deleteBook(id);

        const books = await getAllBooks();
        setBooks(books);
    }
    const openModal = () => {
        setMode(Mode.Create);
        setIsModalOpen(true);
    }
    const closeModal = () => {
        setValues(defaultValues as Book);
        setIsModalOpen(false);
    }
    const openEditModal = (book: Book) => {
        setMode(Mode.Edit);
        setValues(book)
        setIsModalOpen(true);
    }


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
        <div>
            <Button onClick={openModal}>Добавить книгу</Button>

            <CreateUpdateBook
                mode={mode}
                values={values}
                isModalOpen={isModalOpen}
                handleClose={closeModal}
                handleCreateBook={(request: BookRequest, file: File | null) => handleCreateBook(request, file)}
                handleUpdateBook={(id:string, request: BookRequest, file: File | null) => handleUpdateBook(values.id, request, file)}
            />
            
            {loading ? <h1>loading...</h1> : <Books books={books} withEditAndDelete={true} handleOpen={openEditModal} handleDelete={handleDeleteBook}></Books>}
        </div>
    );
}

