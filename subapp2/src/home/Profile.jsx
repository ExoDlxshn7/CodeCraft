import React, { useState, useEffect } from 'react';
import './Profile.css';

const Profile = () => {
    const [posts, setPosts] = useState([]);
    const [postContent, setPostContent] = useState('');
    const [postImage, setPostImage] = useState(null);

    useEffect(() => {
        fetch('/api/posts') 
            .then(response => response.json())
            .then(data => setPosts(data))
            .catch(error => console.error('Error fetching posts:', error));
    }, []);

    const handlePostSubmit = (e) => {
        e.preventDefault();

        const formData = new FormData();
        formData.append('PostContent', postContent);
        formData.append('PostImage', postImage);

        fetch('/api/posts', {
            method: 'POST',
            body: formData,
        })
            .then(response => response.json())
            .then(newPost => setPosts([newPost, ...posts]))
            .catch(error => console.error('Error creating post:', error));
    };

    return (
        <div className="profile-page">
            <div className="profile-container-left">
                <img
                    className="profile-picture"
                    src="https://i.imgur.com/Z8k08or.png"
                    alt="Profile"
                />
                <div className="navbar-left">
                    <a className="navbar-item" href="/account/manage">
                        Edit Profile
                    </a>
                </div>
            </div>

            {/* Høyre panel for innlegg */}
            <div className="profile-container-right">
                {/* Post-innleveringsskjema */}
                <div className="container-post">
                    <form className="post-area" onSubmit={handlePostSubmit}>
                        <textarea
                            className="post-textarea"
                            value={postContent}
                            onChange={(e) => setPostContent(e.target.value)}
                            placeholder="Write something"
                        />
                        <input
                            type="file"
                            className="post-image"
                            accept="image/*"
                            onChange={(e) => setPostImage(e.target.files[0])}
                        />
                        <button className="post-button" type="submit">
                            Post
                        </button>
                    </form>
                </div>

                
                {posts.map((post) => (
                    <div className="container-post" key={post.id}>
                        <div className="post-header">
                            <div className="post-pp">
                                <img
                                    alt="Profile"
                                    className="pp-content"
                                    src={post.user.profilePic}
                                />
                            </div>
                            <div className="post-profile">{post.user.userName}</div>
                            <div className="post-posted">
                                <span>
                                    {new Date(post.createdAt).toLocaleDateString()} <br />
                                    at {new Date(post.createdAt).toLocaleTimeString()}
                                </span>
                            </div>
                        </div>

                        <div className="post-contents">
                            <p>{post.content}</p>
                            {post.imageUrl && (
                                <img
                                    className="photo"
                                    src={post.imageUrl}
                                    alt="Post content"
                                />
                            )}

                            <div className="post-interaction">
                                <span className="post-choice">
                                    <a href="#">
                                        <i className="fa-regular fa-heart"></i> Like
                                    </a>
                                </span>
                                <span className="post-choice">
                                    <a href="#">
                                        <i className="fa-regular fa-comment"></i> Comment
                                    </a>
                                </span>
                                <span className="post-choice">
                                    <a href="#">
                                        <i className="fa-regular fa-star"></i> Favorite
                                    </a>
                                </span>
                                <span className="post-choice">
                                    <a href="#">
                                        <i className="fa-regular fa-share-from-square"></i> Share
                                    </a>
                                </span>
                            </div>

                            <button className="post-edit-button">Edit</button>
                            <button
                                className="post-delete-button"
                                onClick={() => {
                                    if (window.confirm('Are you sure you want to delete this post?')) {
                                        fetch(`/api/posts/${post.id}`, { method: 'DELETE' }) // Endre til riktig API-endepunkt
                                            .then(() =>
                                                setPosts(posts.filter((p) => p.id !== post.id))
                                            );
                                    }
                                }}
                            >
                                Delete
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Profile;
