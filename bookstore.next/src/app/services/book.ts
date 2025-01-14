export interface BookRequest {
    title: string;
    description: string;
    price: number;
    authorId: string;
    categoryId: string;
    stockCount: number;
}

export const getAllBooks = async (): Promise<Book[]> => {
    const response = await fetch("http://localhost:5263/Book/GetBooks");

    if (!response.ok) {
        throw new Error("Failed to fetch books");
    }

    return await response.json();
};

export const createBook = async (bookRequest: BookRequest, file: File) => {
    const formData = new FormData();
    formData.append("Title", bookRequest.title);
    formData.append("Description", bookRequest.description);
    formData.append("Price", bookRequest.price.toString());
    formData.append("AuthorId", bookRequest.authorId);
    formData.append("CategoryId", bookRequest.categoryId);
    formData.append("StockCount", bookRequest.stockCount.toString());
    formData.append("image", file); // Добавляем файл изображения

    await fetch("http://localhost:5263/Book/AddBook", {
        method: "POST",
        body: formData,
    });
};
export const getBookById = async (bookId: string): Promise<Book> => {
    const response = await fetch(`http://localhost:5263/Book/GetBookById?bookId=${bookId}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        }
    })
    if (!response.ok) {
        throw new Error("Failed to fetch book");
    }
    return await response.json();
}

export const updateBook = async (id: string, bookRequest: BookRequest) => {
    await fetch(`http://localhost:5263/Book/UpdateBook?id=${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(bookRequest)
    })
}

export const deleteBook = async (id: string) => {
    await fetch(`http://localhost:5263/Book/DeleteBook?id=${id}`, {
        method: "DELETE"
    })
}