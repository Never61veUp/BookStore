import {Author} from "@/app/models/author";

export const getAllAuthors = async (): Promise<Author[]> => {
    const response = await fetch("http://localhost:5263/api/Author",{
        credentials: 'include',
    });

    if (!response.ok) {
        throw new Error("Failed to fetch books");
    }

    return await response.json();
};