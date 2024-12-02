import React, { useState, useEffect } from "react";

const ProfilePage = () => {
    const [posts, setPosts] = useState([]);
    const [newPost, setNewPost] = useState({ content: "", image: null });

    useEffect(() => {
        fetchPosts();
    }, []);

    const fetchPosts = async () => {
        try {
            const response = await fetch("/api/posts");
            const data = await response.json();
            setPosts(data);
        } catch (error) {
            console.error("Error fetching posts:", error);
        }
    };

    const handleNewPost = async (e) => {
        e.preventDefault();
        const formData = new FormData();
        formData.append("PostContent", newPost.content);
        formData.append("PostImage", newPost.image);

        try {
            const response = await fetch("/api/posts", {
                method: "POST",
                body: formData,
            });
            if (response.ok) {
                fetchPosts(); // Refresh posts
                setNewPost({ content: "", image: null }); // Reset form
            } else {
                console.error("Failed to create post:", response.statusText);
            }
        } catch (error) {
            console.error("Error creating post:", error);
        }
    };

    const handleDeletePost = async (postId) => {
        try {
            const response = await fetch(`/api/posts/${postId}`, {
                method: "DELETE",
            });
            if (response.ok) {
                fetchPosts(); // Refresh posts
            } else {
                console.error("Failed to delete post:", response.statusText);
            }
        } catch (error) {
            console.error("Error deleting post:", error);
        }
    };

    return (
        <div>
            <h1>Profile Page</h1>

            {/* Form to create a new post */}
            <form onSubmit={handleNewPost}>
                <textarea
                    value={newPost.content}
                    onChange={(e) => setNewPost({ ...newPost, content: e.target.value })}
                    placeholder="Write something"
                />
                <input
                    type="file"
                    accept="image/*"
                    onChange={(e) => setNewPost({ ...newPost, image: e.target.files[0] })}
                />
                <button type="submit">Post</button>
            </form>

            {/* Display posts */}
            <div>
                {posts.map((post) => (
                    <div key={post.id}>
                        <div>
                            <img
                                src={post.users?.profilePic || "/Images/default-profile.jpg"}
                                alt="Profile"
                                width="50"
                            />
                            <span>{post.users?.userName || "Unknown User"}</span>
                            <span>{new Date(post.createdAt).toLocaleString()}</span>
                        </div>
                        <p>{post.content}</p>
                        {post.imageUrl && <img src={post.imageUrl} alt="Post" width="200" />}

                        <button onClick={() => handleDeletePost(post.id)}>Delete Post</button>

                        <CommentSection postId={post.id} />
                    </div>
                ))}
            </div>
        </div>
    );
};

const CommentSection = ({ postId }) => {
    const [comments, setComments] = useState([]);
    const [newComment, setNewComment] = useState("");

    useEffect(() => {
        fetchComments();
    }, []);

    const fetchComments = async () => {
        try {
            const response = await fetch(`/api/posts/${postId}/comments`);
            const data = await response.json();
            setComments(data);
        } catch (error) {
            console.error("Error fetching comments:", error);
        }
    };

    const handleNewComment = async (e) => {
        e.preventDefault();
        try {
            const response = await fetch(`/api/posts/${postId}/comments`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ comments: newComment }),
            });
            if (response.ok) {
                fetchComments(); // Refresh comments
                setNewComment(""); // Reset form
            } else {
                console.error("Failed to add comment:", response.statusText);
            }
        } catch (error) {
            console.error("Error adding comment:", error);
        }
    };

    return (
        <div>
            <form onSubmit={handleNewComment}>
                <textarea
                    value={newComment}
                    onChange={(e) => setNewComment(e.target.value)}
                    placeholder="Write a comment..."
                />
                <button type="submit">Add Comment</button>
            </form>

            {comments.map((comment) => (
                <div key={comment.id}>
                    <span>{comment.users?.userName || "Unknown User"}</span>
                    <p>{comment.comments}</p>
                </div>
            ))}
        </div>
    );
};

export default ProfilePage;
