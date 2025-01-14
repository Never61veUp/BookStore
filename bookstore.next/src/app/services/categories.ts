
export const getAllCategories = async (): Promise<Category[]> => {
    const response = await fetch("http://localhost:5263/Categories/GetCategories");

    if (!response.ok) {
        throw new Error("Failed to fetch books");
    }

    return await response.json();
};