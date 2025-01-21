import {
    Card,
    CardContent,
    CardFooter,
    CardHeader,
} from "@/components/ui/card"
import Link from "next/link";
import {Button} from "@/components/ui/button";
import {BookCard} from "@/components/shared/BookCard";
interface Props {
    books: Book[];
    withEditAndDelete: boolean;
    handleDelete?: (id: string) => void;
    handleOpen?: (book: Book) => void;
}

export const Books = ({books, withEditAndDelete, handleDelete, handleOpen}:Props) => {
    return (
        <div className="p-4 grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6">
            {books && books.map((book:Book) => (
                <BookCard key={book.id} book={book} withEditAndDelete={withEditAndDelete}
                          handleDelete={handleDelete} handleOpen={handleOpen} />
            ))}
        </div>
    )
}