
export const getAllCategories = async (): Promise<Category[]> => {
    const response = await fetch("http://localhost:5263/api/Categories");

    if (!response.ok) {
        throw new Error("Failed to fetch books");
    }

    return await response.json();
};