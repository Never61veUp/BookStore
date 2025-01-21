import {Card, CardContent, CardFooter, CardHeader} from "@/components/ui/card";
import Link from "next/link";
import {Button} from "@/components/ui/button";

interface Props {
    book: Book;
    withEditAndDelete: boolean;
    handleDelete?: (id: string) => void;
    handleOpen?: (book: Book) => void;
}
export const BookCard = ({book, withEditAndDelete, handleDelete, handleOpen}:Props) => {
    return (
        <Card key={book.id} className="max-w-xs bg-white shadow-lg">
            <CardHeader className="p-0">
                <img
                    src={book.image.url}

                    className="w-full h-64 object-cover rounded-t-lg"
                    alt={book.title}/>
            </CardHeader>

            <CardContent className="p-4">
                <h3 className="text-xl font-semibold text-gray-800 truncate">{book.title}</h3>
                <p className="text-sm text-gray-500 mb-4">by {book.authorFullName.firstName}</p>
                <div className="text-lg font-semibold text-gray-800 mb-4">${book.price.value}</div>
            </CardContent>

            <CardFooter className="p-4 flex flex-col items-center space-y-4">
                <Link href={`/catalog/${book.id}`}>
                    <p className="px-6 block w-full text-center text-white bg-blue-600 hover:bg-blue-700 py-2 rounded-md transition-colors">
                        Подробнее
                    </p>
                </Link>

                {withEditAndDelete && (
                    <div className="flex w-full justify-center space-x-4">
                        <Button
                            onClick={handleDelete != undefined ? () => handleDelete(book.id) : undefined}
                            className="bg-red-600 hover:bg-red-700 text-white font-semibold py-2 rounded-md transition-colors"
                        >
                            Удалить
                        </Button>
                        <Button
                            onClick={handleOpen != undefined ? () => handleOpen(book) : undefined}
                            className="bg-gray-600 hover:bg-gray-700 text-white font-semibold py-2 rounded-md transition-colors"
                        >
                            Редактировать
                        </Button>
                    </div>
                )}
            </CardFooter>
        </Card>
    )
}