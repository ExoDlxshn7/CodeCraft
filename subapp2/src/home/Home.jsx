import React, { useState, useEffect } from "react";

const HomePage = () => {
    const [posts, setPosts] = useState([]);

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

    const handleAddComment = async (postId, comment) => {
        try {
            const response = await fetch(`/api/posts/${postId}/comments`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ comments: comment }),
            });
            if (response.ok) {
                fetchPosts(); // Refresh posts
            } else {
                console.error("Failed to add comment:", response.statusText);
            }
        } catch (error) {
            console.error("Error adding comment:", error);
        }
    };

    const handleDeleteComment = async (commentId) => {
        try {
            const response = await fetch(`/api/comments/${commentId}`, {
                method: "DELETE",
            });
            if (response.ok) {
                fetchPosts(); // Refresh posts
            } else {
                console.error("Failed to delete comment:", response.statusText);
            }
        } catch (error) {
            console.error("Error deleting comment:", error);
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
        <div id="container-full">
            {posts.map((post) => (
                <div id="container-post" key={post.id}>
                    <div id="post-header">
                        <div id="post-pp">
                            <img
                                alt="Profile picture"
                                id="pp-content"
                                src={post.users?.profilePic || "/Images/default-profile.jpg"}
                            />
                        </div>
                        <div id="post-profile">
                            <span>{post.users?.userName || "Unknown User"}</span>
                        </div>
                        <div id="post-posted">
                            <span>
                                {new Date(post.createdAt).toLocaleDateString()} <br />
                                {new Date(post.createdAt).toLocaleTimeString()}
                            </span>
                        </div>
                    </div>
                    <div id="post-contents">
                        <p>{post.content}</p>
                        {post.imageUrl && <img id="photo" src={post.imageUrl} alt="Post" />}
                        <CommentSection
                            postId={post.id}
                            comments={post.comments}
                            onAddComment={handleAddComment}
                            onDeleteComment={handleDeleteComment}
                        />
                        <button onClick={() => handleDeletePost(post.id)}>Delete Post</button>
                    </div>
                </div>
            ))}
        </div>
    );
};

const CommentSection = ({ postId, comments, onAddComment, onDeleteComment }) => {
    const [newComment, setNewComment] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();
        if (newComment.trim()) {
            onAddComment(postId, newComment);
            setNewComment("");
        }
    };

    return (
        <div>
            <form onSubmit={handleSubmit}>
                <textarea
                    value={newComment}
                    onChange={(e) => setNewComment(e.target.value)}
                    placeholder="Write a comment..."
                />
                <button type="submit">Add Comment</button>
            </form>
            <div>
                {comments.map((comment) => (
                    <div id="comments-section" key={comment.id}>
                        <img
                            alt="Profile picture"
                            id="pp-content"
                            src={comment.users?.profilePic || "/Images/default-profile.jpg"}
                        />
                        <b>{comment.users?.userName || "Unknown User"}</b>
                        <span>
                            {new Date(comment.createdAt).toLocaleDateString()} at{" "}
                            {new Date(comment.createdAt).toLocaleTimeString()}
                        </span>
                        <p>{comment.comments}</p>
                        <button onClick={() => onDeleteComment(comment.id)}>Delete Comment</button>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default HomePage;
